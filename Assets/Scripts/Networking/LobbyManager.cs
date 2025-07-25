using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using static UnityEngine.EventSystems.StandaloneInputModule;



public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }
    public string joinCode;
    public Lobby CurrentLobby { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private async void Start()
    {
        GameEvents.OnCreateLobbyRequested += CreateLobby;
        GameEvents.OnJoinLobbyRequested += JoinLobby;

        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        Debug.Log("Zalogowano jako: " + AuthenticationService.Instance.PlayerId);
    }
    private void OnDestroy()
    {
        GameEvents.OnCreateLobbyRequested -= CreateLobby;
        GameEvents.OnJoinLobbyRequested -= JoinLobby;
    }
    private string playerName => $"Player_{AuthenticationService.Instance.PlayerId.Substring(0, 6)}";
    private async void CreateLobby()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        

        var options = new CreateLobbyOptions
        {
            Player = new Player
            {
                Data = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
            }
            },
            Data = new Dictionary<string, DataObject>
        {
            { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
        }
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("ChessLobby", 2, options);
        await LobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions
        {
            Data = new Dictionary<string, DataObject>
    {
        { "joinCode", new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
    }
        });
        GameEvents.RequestUpdateLobbyCode(lobby.LobbyCode);
        CurrentLobby = lobby;

        Debug.Log("Lobby utworzone:");
        foreach (var kvp in lobby.Data)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value.Value}");
        }
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetHostRelayData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );

        NetworkManager.Singleton.StartHost();
    }
    private async void JoinLobby(string lobbyCode)
    {
        var joinOptions = new JoinLobbyByCodeOptions
        {
            Player = new Player
            {
                Data = new Dictionary<string, PlayerDataObject>
            {
                { "name", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
            }
            }
        };

        Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, joinOptions);
        CurrentLobby = lobby;

        if (!lobby.Data.ContainsKey("joinCode") || string.IsNullOrWhiteSpace(lobby.Data["joinCode"].Value))
        {
            Debug.LogError("Brak kodu Relay w danych lobby!");
            return;
        }

        string relayJoinCode = lobby.Data["joinCode"].Value;

        JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayJoinCode);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetClientRelayData(
            joinAllocation.RelayServer.IpV4,
            (ushort)joinAllocation.RelayServer.Port,
            joinAllocation.AllocationIdBytes,
            joinAllocation.Key,
            joinAllocation.ConnectionData,
            joinAllocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();
    }

    public async void LeaveLobby()
    {
        try
        {
            if (CurrentLobby == null)
            {
                Debug.LogWarning("LeaveLobby: brak aktywnego lobby.");
                return;
            }

            string playerId = AuthenticationService.Instance.PlayerId;
            bool isHost = CurrentLobby.HostId == playerId;

            if (isHost)
            {
                await LobbyService.Instance.DeleteLobbyAsync(CurrentLobby.Id);
                Debug.Log("LeaveLobby: Host usun¹³ lobby.");
                GameEvents.RequestCloseLobby();
            }
            else
            {
                await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, playerId);
                Debug.Log("LeaveLobby: Klient opuœci³ lobby.");
                GameEvents.RequestLeaveLobby();
            }

            CurrentLobby = null;
            joinCode = null;

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError("LeaveLobby error: " + ex.Message);
        }
    }
}
