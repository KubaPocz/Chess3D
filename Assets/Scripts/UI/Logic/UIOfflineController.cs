using Core.Config;
using Core.Settings;
using Core.Utilities;
using Game;
using Game.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Logic
{
    public class UIOfflineController : MonoBehaviour
    {
        [Header("InGamePanel")]
        [SerializeField] GameObject Panel;
        [SerializeField] TextMeshProUGUI TurnLabel;
        [SerializeField] TextMeshProUGUI MovesLabel;
        [SerializeField] TextMeshProUGUI CapturedLabel;
        [SerializeField] TextMeshProUGUI DifficultyLabel;
        [SerializeField] TextMeshProUGUI TimerLabel;
        [Header("InGameMenu")]
        [SerializeField] GameObject MenuPanel;
        [SerializeField] Button RestartGameButton;
        [SerializeField] Button SurrenderGameButton;
        [SerializeField] Button ExitGameButton;
        [Header("InfoLabel")]
        [SerializeField] TextMeshProUGUI CheckLabel;
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
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameEvents.RequestPauseGame();
            }
        }
        void OnDestroy()
        {
            GameEvents.OnChangeTurnRequested -= UpdateTurnLabel;
            GameEvents.OnAddPlayerMoveRequested -= UpdateMovesCountLabel;
            GameEvents.OnPauseGameRequested -= PauseGame;
            GameEvents.OnRestartGameRequested -= RestartGame;
        }
        void BootLabels()
        {
            if (GameSettingsStore.CurrentSettings.HostColor == GameStats.Instance.CurrentTurnColor)
                TurnLabel.text = "Player Turn";
            else
                TurnLabel.text = "Bot Turn";
            MovesLabel.text = 0.ToString();
            CapturedLabel.text = 0.ToString();
            string difficulty;
            int value = GameSettingsStore.CurrentSettings.Difficulty;
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
        void UpdateTurnLabel()
        {
            if (GameSettingsStore.CurrentSettings.HostColor == GameStats.Instance.CurrentTurnColor)
                TurnLabel.text = "Player Turn";
            else
                TurnLabel.text = "Bot Turn";
        }
        void UpdateMovesCountLabel()
        {
            if (GameSettingsStore.CurrentSettings.HostColor == ChessColor.White)
                MovesLabel.text = GameStats.Instance.WhiteMoves.ToString();
            else
                MovesLabel.text = GameStats.Instance.BlackMoves.ToString();
        }
        void PauseGame()
        {
            Panel.SetActive(!Panel.activeInHierarchy);
            MenuPanel.SetActive(!MenuPanel.activeInHierarchy);
        }
        void RestartGame()
        {
            GameEvents.RequestStartGameOffline(GameSettingsStore.CurrentSettings.HostColor, GameSettingsStore.CurrentSettings.Difficulty);
        }
    }
}
