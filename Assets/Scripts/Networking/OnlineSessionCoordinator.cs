using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class OnlineSessionCoordinator : NetworkBehaviour
{
    public static OnlineSessionCoordinator Instance { get; private set; }
    [SerializeField] UnityTransport transport;
    [SerializeField] string gameSceneName = "Game";
    public ChessColor hostColor;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        GameEvents.HostAllocateRelayRequested += OnHostAllocateRelayRequested;
        GameEvents.ClientJoinRelayRequested += OnClientJoinRelayRequested;

        GameEvents.OnMovePieceOnlineRequested += MovePieceServerRpc;
        GameEvents.OnRefreshBoardVisualsOnline += RefreshBoardVisualsClientRpc;
    }
    void OnDisable()
    {
        GameEvents.HostAllocateRelayRequested -= OnHostAllocateRelayRequested;
        GameEvents.ClientJoinRelayRequested -= OnClientJoinRelayRequested;

        GameEvents.OnMovePieceOnlineRequested -= MovePieceServerRpc;
        GameEvents.OnRefreshBoardVisualsOnline -= RefreshBoardVisualsClientRpc;
    }

    async Task<string> OnHostAllocateRelayRequested(int expectedClients)
    {
        try
        {
            var alloc = await RelayService.Instance.CreateAllocationAsync(expectedClients);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(alloc.AllocationId);

            var serverData = AllocationUtils.ToRelayServerData(alloc, "dtls");
            transport.SetRelayServerData(serverData);

            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                NetworkManager.Singleton.StartHost();

            // Host ³aduje scenê przez NGO – klienci zsynchronizuj¹ siê automatycznie
            NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);

            return joinCode;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Host allocate/start failed: " + ex.Message);
            return null;
        }
    }

    async Task OnClientJoinRelayRequested(string joinCode)
    {
        try
        {
            var joinAlloc = await RelayService.Instance.JoinAllocationAsync(joinCode);
            var clientData = AllocationUtils.ToRelayServerData(joinAlloc, "dtls");
            transport.SetRelayServerData(clientData);

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
                NetworkManager.Singleton.StartClient();
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Client join failed: " + ex.Message);
        }
    }
    [ServerRpc]
    public void MovePieceServerRpc(string uci)
    {
        GameManager.Instance.MovePiece(uci);
    }

    [ClientRpc]
    public void RefreshBoardVisualsClientRpc()
    {
        BoardManager.Instance.RefreshBoardVisuals();
    }
}
