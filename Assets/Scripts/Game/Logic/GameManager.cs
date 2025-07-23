using UnityEngine;
using UnityEngine.SceneManagement;
using static Codice.CM.Common.CmCallContext;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IPlayerController whitePlayer;
    private IPlayerController blackPlayer;
    public IPlayerController CurrentPlayer
    {
        get => GameStats.Instance.currentTurnColor == ChessColor.White ? whitePlayer : blackPlayer;
    }
    public IPlayerController WaitingPlayer => (CurrentPlayer == whitePlayer) ? blackPlayer : whitePlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Debug.Log($"[GameManager.Start] player1 = {GameSetupManager.Instance?.player1}");
        Debug.Log($"[GameManager.Start] player2 = {GameSetupManager.Instance?.player2}");

        GameEvents.OnPauseGameRequested += PasueGame;
        GameEvents.OnExitGameRequested += ExitGame;

        var setup = GameSetupManager.Instance;

        ChessColor player1Color = GameConfigStore.CurrentConfig.PlayerColor;

        if (player1Color == ChessColor.White)
        {
            AssignPlayers(setup.player1, setup.player2);
        }
        else
        {
            AssignPlayers(setup.player2, setup.player1);
        }
        Debug.Log($"{whitePlayer.ToString()}, {blackPlayer.ToString()}");
        StartGame();
    }
    private void OnDestroy()
    {
        GameEvents.OnPauseGameRequested -= PasueGame;
        GameEvents.OnExitGameRequested -= ExitGame;

        Instance = null;
        whitePlayer = null;
        blackPlayer = null;
    }
    public void AssignPlayers(IPlayerController white, IPlayerController black)
    {
        whitePlayer = white;
        blackPlayer = black;
    }
    public void StartGame()
    {
        blackPlayer.EndTurn();

        CurrentPlayer.StartTurn();
    }

    public void OnMoveCompleted()
    {
        CurrentPlayer.EndTurn();

        SwitchTurn();

        CurrentPlayer.StartTurn();
        Debug.Log("onmovecompleted");
    }

    private void SwitchTurn()
    {
        GameEvents.RequestChangeTurn();
        Debug.Log("Kolej gracza: " + GameStats.Instance.currentTurnColor);
    }
    public bool IsCurrentTurn(ChessColor color) => color == GameStats.Instance.currentTurnColor;
    private void PasueGame()
    {
        var player = CurrentPlayer as MonoBehaviour;

        if (player == null)
        {
            Debug.LogWarning("❌ PauseGame: CurrentPlayer is null or destroyed.");
            return;
        }

        if (player.gameObject == null || !player.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("❌ PauseGame: CurrentPlayer's GameObject is inactive or destroyed.");
            return;
        }

        player.enabled = !player.enabled;
        Debug.Log($"[PauseGame] Toggled player.enabled = {player.enabled}");
    }

    private void ExitGame()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        if (GameSetupManager.Instance != null)
        {
            Destroy(GameSetupManager.Instance.gameObject);
        }

        SceneLoader.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }

}
