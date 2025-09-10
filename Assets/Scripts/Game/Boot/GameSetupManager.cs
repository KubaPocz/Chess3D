using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSetupManager : MonoBehaviour
{
    public static GameSetupManager Instance { get; private set; }

    [SerializeField] public GameObject humanPrefab;
    [SerializeField] public GameObject botPrefab;
    [SerializeField] public Camera whiteCamera;
    [SerializeField] public Camera blackCamera;

    public IPlayerController player1 { get; private set; }
    public IPlayerController player2 { get; private set; }
    private void Awake()
    {
        Debug.Log($"[GameSetupManager.Awake] executing at t={Time.time}");

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        switch (GameConfigStore.CurrentConfig.GameMode)
        {
            case (GameMode.HumanVsHuman):
                player1 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                player2 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                SceneManager.LoadScene("UI_Online", LoadSceneMode.Additive);
                break;
            case (GameMode.HumanVsBot):
                player1 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                player2 = Instantiate(botPrefab).GetComponent<IPlayerController>();
                SceneManager.LoadScene("UI_Offline", LoadSceneMode.Additive);
                //setting cameras
                whiteCamera.gameObject.SetActive(GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White);
                blackCamera.gameObject.SetActive(GameConfigStore.CurrentConfig.PlayerColor == ChessColor.Black);

                break;
            default:
                throw new System.Exception("Unsupported game mode.");
        }
        ChessColor player1Color = GameConfigStore.CurrentConfig.PlayerColor;
        ChessColor player2Color = GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White ? ChessColor.Black : ChessColor.White;

        player1.Initialize(player1Color);
        player2.Initialize(player2Color);
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
