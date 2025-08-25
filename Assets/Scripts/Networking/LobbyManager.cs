// asmdef: Game.Networking
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class LobbyManager : MonoBehaviour
{
    string lobbyId;
    bool isHost;
    CancellationTokenSource heartbeatCts;

    private ILobbyEvents lobbyEvents;
    private Lobby currentLobby;

    void Awake() => DontDestroyOnLoad(gameObject);

    async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    void OnEnable()
    {
        GameEvents.CreateLobbyRequested += OnCreateLobbyRequested;
        GameEvents.JoinLobbyByCodeRequested += OnJoinByCodeRequested;
        GameEvents.LeaveOrDeleteLobbyRequested += OnLeaveOrDeleteRequested;
        GameEvents.OnSwapTeamsRequested += SwapTeams;
    }

    void OnDisable()
    {
        GameEvents.CreateLobbyRequested -= OnCreateLobbyRequested;
        GameEvents.JoinLobbyByCodeRequested -= OnJoinByCodeRequested;
        GameEvents.LeaveOrDeleteLobbyRequested -= OnLeaveOrDeleteRequested;
        GameEvents.OnSwapTeamsRequested -= SwapTeams;
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

            isHost = true;
            lobbyId = lobby.Id;
            currentLobby = lobby;

            GameEvents.NotifyCreated(lobby.Id, lobby.LobbyCode);
            RefreshPlayersUI();

            heartbeatCts?.Cancel();
            heartbeatCts = new CancellationTokenSource();
            _ = RunHeartbeat(lobby.Id, heartbeatCts.Token);

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
            GameEvents.NotifyError("Nieprawid³owy kod lobby.");
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

            isHost = false;
            lobbyId = lobby.Id;
            currentLobby = lobby;

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
            if (!isHost || currentLobby == null || currentLobby.Data == null || currentLobby.Players.Count != 2) return;

            string hostColor = currentLobby.Data.TryGetValue("hostColor", out var hc) ? hc.Value : "White";
            string clientColor = currentLobby.Data.TryGetValue("clientColor", out var cc) ? cc.Value : "Black";

            await LobbyService.Instance.UpdateLobbyAsync(currentLobby.Id, new UpdateLobbyOptions
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

    async void OnLeaveOrDeleteRequested()
    {
        try
        {
            heartbeatCts?.Cancel();
            if (!string.IsNullOrEmpty(lobbyId))
            {
                if (isHost)
                {
                    await LobbyService.Instance.DeleteLobbyAsync(lobbyId);
                    GameEvents.NotifyLeftOrDeleted();
                }
                else
                {
                    await LobbyService.Instance.RemovePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId);
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
            isHost = false;
            lobbyId = null;
            currentLobby = null;
            if (lobbyEvents != null)
            {
                await lobbyEvents.UnsubscribeAsync();
                lobbyEvents = null;
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
        if (lobbyEvents != null)
        {
            await lobbyEvents.UnsubscribeAsync();
            lobbyEvents = null;
        }

        var callbacks = new LobbyEventCallbacks();
        callbacks.LobbyChanged += OnLobbyChanged;
        callbacks.LobbyDeleted += OnLobbyDeleted;
        callbacks.KickedFromLobby += OnKickedFromLobby;
        callbacks.LobbyEventConnectionStateChanged += OnLobbyEventConnectionStateChanged;

        lobbyEvents = await LobbyService.Instance.SubscribeToLobbyEventsAsync(lobbyId, callbacks);
        await lobbyEvents.SubscribeAsync();
    }

    void OnLobbyChanged(ILobbyChanges changes)
    {
        if (currentLobby == null) return;
        changes.ApplyToLobby(currentLobby);
        RefreshPlayersUI();
    }

    void OnLobbyDeleted()
    {
        if (!isHost)
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
        if (currentLobby == null) return;

        var players = new List<string>();
        var seenNames = new HashSet<string>();

        string hostColor = currentLobby.Data.TryGetValue("hostColor", out var hc) ? hc.Value : "?";
        string clientColor = currentLobby.Data.TryGetValue("clientColor", out var cc) ? cc.Value : "?";

        foreach (var p in currentLobby.Players)
        {
            string name = p.Data != null && p.Data.TryGetValue("name", out var n) ? n.Value : "Anon";

            if (seenNames.Contains(name))
                name = name + "*";

            string color = "?";

            // jeœli to gracz hosta
            if (p.Id == currentLobby.HostId)
                color = hostColor;
            else
                color = clientColor;

            players.Add($"{name} ({color})");
            seenNames.Add(name);
        }

        GameEvents.NotifyPlayersListUpdated(players);
    }

}
