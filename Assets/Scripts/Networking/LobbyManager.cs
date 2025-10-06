// asmdef: Game.Networking

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Boot;
using Core.Config;
using Core.Settings;
using Core.Utilities;
using Game;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Networking
{
    public class LobbyManager : MonoBehaviour
    {
        string _lobbyId;
        bool _isHost;
        CancellationTokenSource _heartbeatCts;

        ILobbyEvents _lobbyEvents;
        Lobby _currentLobby;
        const string KeyStarted = "started";
        const string KeyJoincode = "joinCode";
        void Awake() => DontDestroyOnLoad(gameObject);

        async void Start()
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            GameSettingsStore.CurrentSettings = new GameSettings(GameMode.HumanVsHuman, ChessColor.White);
        }

        void OnEnable()
        {
            GameEvents.CreateLobbyRequested += OnCreateLobbyRequested;
            GameEvents.JoinLobbyByCodeRequested += OnJoinByCodeRequested;
            GameEvents.LeaveOrDeleteLobbyRequested += OnLeaveOrDeleteRequested;
            GameEvents.OnSwapTeamsRequested += SwapTeams;
            GameEvents.OnStartGameOnlineRequested += StartGameOnline;
        }

        void OnDisable()
        {
            GameEvents.CreateLobbyRequested -= OnCreateLobbyRequested;
            GameEvents.JoinLobbyByCodeRequested -= OnJoinByCodeRequested;
            GameEvents.LeaveOrDeleteLobbyRequested -= OnLeaveOrDeleteRequested;
            GameEvents.OnSwapTeamsRequested -= SwapTeams;
            GameEvents.OnStartGameOnlineRequested -= StartGameOnline;

        }

        async void OnCreateLobbyRequested()
        {
            try
            {
                string localName = PlayerPrefs.GetString("PlayerName");

                var lobby = await LobbyService.Instance.CreateLobbyAsync(
                    "Lobby",
                    2,
                    new CreateLobbyOptions
                    {
                        IsPrivate = false,
                        Player = new Player
                        {
                            Data = new Dictionary<string, PlayerDataObject>
                            {
                                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, localName) }
                            }
                        },
                        Data = new Dictionary<string, DataObject>
                        {
                            { "hostColor", new DataObject(DataObject.VisibilityOptions.Member, "White") },
                            { "clientColor", new DataObject(DataObject.VisibilityOptions.Member, "Black") }
                        }
                    });

                _isHost = true;
                _lobbyId = lobby.Id;
                _currentLobby = lobby;

                GameEvents.NotifyCreated(lobby.Id, lobby.LobbyCode);
                RefreshPlayersUI();

                _heartbeatCts?.Cancel();
                _heartbeatCts = new CancellationTokenSource();
                _ = RunHeartbeat(lobby.Id, _heartbeatCts.Token);

                await SubscribeToLobbyEvents(lobby.Id);
            }
            catch (LobbyServiceException e)
            {
                GameEvents.NotifyError($"Create failed: {e.Reason}");
            }
        }

        async void OnJoinByCodeRequested(string code)
        {
            var c = code?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(c) || c.Length < 6)
            {
                GameEvents.NotifyError("Nieprawid�owy kod lobby.");
                return;
            }

            try
            {
                string localName = PlayerPrefs.GetString("PlayerName");

                var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(
                    c,
                    new JoinLobbyByCodeOptions
                    {
                        Player = new Player
                        {
                            Data = new Dictionary<string, PlayerDataObject>
                            {
                                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, localName) }
                            }
                        }
                    });

                _isHost = false;
                _lobbyId = lobby.Id;
                _currentLobby = lobby;

                GameEvents.NotifyJoined(lobby.Id, lobby.LobbyCode);
                RefreshPlayersUI();

                await SubscribeToLobbyEvents(lobby.Id);
            }
            catch (LobbyServiceException e)
            {
                GameEvents.NotifyError($"Join failed: {e.Reason}");
            }
        }

        public async void SwapTeams()
        {
            try
            {
                if (!_isHost || _currentLobby == null || _currentLobby.Data == null || _currentLobby.Players.Count != 2) return;

                string hostColor = _currentLobby.Data.TryGetValue("hostColor", out var hc) ? hc.Value : "White";
                string clientColor = _currentLobby.Data.TryGetValue("clientColor", out var cc) ? cc.Value : "Black";
                if (OnlineSessionCoordinator.Instance.hostColor == ChessColor.White)
                    OnlineSessionCoordinator.Instance.hostColor = ChessColor.Black;
                else
                    OnlineSessionCoordinator.Instance.hostColor = ChessColor.White;
                await LobbyService.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { "hostColor", new DataObject(DataObject.VisibilityOptions.Member, clientColor) },
                        { "clientColor", new DataObject(DataObject.VisibilityOptions.Member, hostColor) }
                    }
                });
            }
            catch
            {

            }
        }
        public async void StartGameOnline()
        {
            if (!_isHost || _currentLobby == null)
            {
                GameEvents.NotifyError("Tylko host mo�e rozpocz�� gr�.");
                return;
            }
            if (_currentLobby.Players == null || _currentLobby.Players.Count != 2)
            {
                GameEvents.NotifyError("Ta gra wymaga dok�adnie 2 graczy.");
                return;
            }
            try
            {
                Debug.Log($"[LobbyManager.StartGameOnline] executing at t={Time.time}");
                string joinCode = await GameEvents.RequestHostAllocateRelayAsync(1);
                GameSettingsStore.CurrentSettings.HostColor = OnlineSessionCoordinator.Instance.hostColor;
                if (string.IsNullOrEmpty(joinCode))
                {
                    GameEvents.NotifyError("Nie uda�o si� przygotowa� sesji sieciowej (Relay).");
                    return;
                }
                await LobbyService.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { KeyStarted,  new DataObject(DataObject.VisibilityOptions.Member, "true") },
                        { KeyJoincode, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
                    }
                });
            }
            catch (LobbyServiceException e)
            {
                GameEvents.NotifyError($"StartGameOnline failed (Lobby): {e.Reason}");
            }
            catch (System.Exception ex)
            {
                GameEvents.NotifyError($"StartGameOnline failed: {ex.Message}");
            }
        }


        async void OnLeaveOrDeleteRequested()
        {
            try
            {
                _heartbeatCts?.Cancel();

                if (!string.IsNullOrEmpty(_lobbyId))
                {
                    if (_isHost)
                    {
                        await LobbyService.Instance.DeleteLobbyAsync(_lobbyId);
                        GameEvents.NotifyLeftOrDeleted();
                    }
                    else
                    {
                        await LobbyService.Instance.RemovePlayerAsync(_lobbyId, AuthenticationService.Instance.PlayerId);
                        GameEvents.NotifyLeftOrDeleted();
                    }
                }
            }
            catch (LobbyServiceException e)
            {
                GameEvents.NotifyError($"Leave/Delete failed: {e.Reason}");
            }
            finally
            {
                _isHost = false;
                _lobbyId = null;
                _currentLobby = null;

                if (_lobbyEvents != null)
                {
                    try
                    {
                        await _lobbyEvents.UnsubscribeAsync();
                    }
                    catch (ObjectDisposedException)
                    {
                        Debug.LogWarning("Tried to unsubscribe from a disposed lobby (already cleaned up).");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Unexpected error unsubscribing: {ex}");
                    }
                    finally
                    {
                        _lobbyEvents = null;
                    }
                }
            }
        }

        async Task RunHeartbeat(string id, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try { await LobbyService.Instance.SendHeartbeatPingAsync(id); }
                catch { break; }
                try { await Task.Delay(15000, ct); } catch { break; }
            }
        }

        async Task SubscribeToLobbyEvents(string lobbyId)
        {
            if (_lobbyEvents != null)
            {
                await _lobbyEvents.UnsubscribeAsync();
                _lobbyEvents = null;
            }

            var callbacks = new LobbyEventCallbacks();
            callbacks.LobbyChanged += OnLobbyChanged;
            callbacks.LobbyDeleted += OnLobbyDeleted;
            callbacks.KickedFromLobby += OnKickedFromLobby;
            callbacks.LobbyEventConnectionStateChanged += OnLobbyEventConnectionStateChanged;

            _lobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyId, callbacks);
            await _lobbyEvents.SubscribeAsync();
        }

        void OnLobbyChanged(ILobbyChanges changes)
        {
            if (_currentLobby == null) return;
            changes.ApplyToLobby(_currentLobby);
            RefreshPlayersUI();

            if (!_isHost &&
                _currentLobby.Data != null &&
                _currentLobby.Data.TryGetValue(KeyStarted, out var startedObj) &&
                startedObj.Value == "true" &&
                _currentLobby.Data.TryGetValue(KeyJoincode, out var jcObj) &&
                !string.IsNullOrEmpty(jcObj.Value))
            {
                // Popros koordynatora sieci o JoinRelay + StartClient
                _ = GameEvents.RequestClientJoinRelayAsync(jcObj.Value);
            }
        }

        void OnLobbyDeleted()
        {
            if (!_isHost)
            {
                GameEvents.NotifyLobbyClosedByHost();
            }
        }

        void OnKickedFromLobby()
        {
            GameEvents.NotifyLeftOrDeleted();
        }

        void OnLobbyEventConnectionStateChanged(LobbyEventConnectionState state)
        {
            Debug.Log($"Lobby event connection state: {state}");
        }

        void OnApplicationQuit() => OnLeaveOrDeleteRequested();
        void OnDestroy() => OnLeaveOrDeleteRequested();

        void RefreshPlayersUI()
        {
            if (_currentLobby == null) return;

            var players = new List<string>();
            var seenNames = new HashSet<string>();

            string hostColor = _currentLobby.Data.TryGetValue("hostColor", out var hc) ? hc.Value : "?";
            string clientColor = _currentLobby.Data.TryGetValue("clientColor", out var cc) ? cc.Value : "?";

            foreach (var p in _currentLobby.Players)
            {
                string name = p.Data != null && p.Data.TryGetValue("name", out var n) ? n.Value : "Anon";

                if (seenNames.Contains(name))
                    name = name + "*";

                string color = "?";

                // je�li to gracz hosta
                if (p.Id == _currentLobby.HostId)
                    color = hostColor;
                else
                    color = clientColor;

                players.Add($"{name} ({color})");
                seenNames.Add(name);
            }

            GameEvents.NotifyPlayersListUpdated(players);
        }

    }
}
