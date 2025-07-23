using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UI;

public class UIOfflineController : MonoBehaviour
{
    [Header("InGamePanel")]
    [SerializeField] private GameObject Panel;
    [SerializeField] private TextMeshProUGUI TurnLabel;
    [SerializeField] private TextMeshProUGUI MovesLabel;
    [SerializeField] private TextMeshProUGUI CapturedLabel;
    [SerializeField] private TextMeshProUGUI DifficultyLabel;
    [SerializeField] private TextMeshProUGUI TimerLabel;
    [Header("InGameMenu")]
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private Button RestartGameButton;
    [SerializeField] private Button SurrenderGameButton;
    [SerializeField] private Button ExitGameButton;
    [Header("InfoLabel")]
    [SerializeField] private TextMeshProUGUI CheckLabel;
    void Start()
    {
        GameEvents.OnChangeTurnRequested += UpdateTurnLabel;
        GameEvents.OnAddPlayerMoveRequested += UpdateMovesCountLabel;
        GameEvents.OnPauseGameRequested += PauseGame;
        GameEvents.OnRestartGameRequested += RestartGame;

        RestartGameButton.onClick.AddListener(() => GameEvents.RequestRestartGame());
        SurrenderGameButton.onClick.AddListener(() => GameEvents.RequestSurrenderGame());
        ExitGameButton.onClick.AddListener(() => GameEvents.RequestExitGame());

        Panel.SetActive(true);
        MenuPanel.SetActive(false);

        BootLabels();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEvents.RequestPauseGame();
        }
    }
    private void OnDestroy()
    {
        GameEvents.OnChangeTurnRequested -= UpdateTurnLabel;
        GameEvents.OnAddPlayerMoveRequested -= UpdateMovesCountLabel;
        GameEvents.OnPauseGameRequested -= PauseGame;
        GameEvents.OnRestartGameRequested -= RestartGame;
    }
    private void BootLabels()
    {
        if (GameConfigStore.CurrentConfig.PlayerColor == GameStats.Instance.currentTurnColor)
            TurnLabel.text = "Player Turn";
        else
            TurnLabel.text = "Bot Turn";
        MovesLabel.text = 0.ToString();
        CapturedLabel.text = 0.ToString();
        string difficulty;
        int value = GameConfigStore.CurrentConfig.Difficulty;
        if (value <= 2) difficulty = "Very Easy";
        else if (value <= 4) difficulty = "Easy";
        else if (value <= 7) difficulty = "Medium";
        else if (value <= 10) difficulty = "Challenging";
        else if (value <= 13) difficulty = "Hard";
        else if (value <= 16) difficulty = "Very Hard";
        else if (value <= 18) difficulty = "Expert";
        else difficulty = "Master";
        DifficultyLabel.text = difficulty;
        TimerLabel.text = "15:00";
    }
    private void UpdateTurnLabel()
    {
        if (GameConfigStore.CurrentConfig.PlayerColor == GameStats.Instance.currentTurnColor)
            TurnLabel.text = "Player Turn";
        else
            TurnLabel.text = "Bot Turn";
    }
    private void UpdateMovesCountLabel()
    {
        if (GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White)
            MovesLabel.text = GameStats.Instance.whiteMoves.ToString();
        else
            MovesLabel.text = GameStats.Instance.blackMoves.ToString();
    }
    private void UpdateTimerLabel()
    {

    }
    private void PauseGame()
    {
        Panel.SetActive(!Panel.activeInHierarchy);
        MenuPanel.SetActive(!MenuPanel.activeInHierarchy);
    }
    private void RestartGame()
    {
        GameEvents.RequestStartGameOffline(GameConfigStore.CurrentConfig.PlayerColor, GameConfigStore.CurrentConfig.Difficulty);
    }
}
