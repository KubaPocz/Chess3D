using Core.Boot;
using Core.Config;
using Core.Interfaces;
using Core.Settings;
using Core.Utilities;
using Game.Boot;
using Game.Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Logic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        IPlayerController _whitePlayer;
        IPlayerController _blackPlayer;
        public IPlayerController CurrentPlayer
        {
            get => GameStats.Instance.CurrentTurnColor == ChessColor.White ? _whitePlayer : _blackPlayer;
        }
        public IPlayerController WaitingPlayer => (CurrentPlayer == _whitePlayer) ? _blackPlayer : _whitePlayer;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        void Start()
        {

            GameEvents.OnPauseGameRequested += PasueGame;
            GameEvents.OnExitGameRequested += ExitGame;

            GameEvents.OnMovePieceOfflineRequested += MovePiece;

            if (GameSettingsStore.CurrentSettings.GameMode == GameMode.HumanVsBot)
            {
                InitPlayersAndStart();
            }
            else
            {
                // Online -> czekamy aż NGO zespawnuje NetworkPlayerController
                GameSetupManager.OnPlayersReady += (_, _) =>
                {
                    InitPlayersAndStart();
                };
            }
        }
        void OnDestroy()
        {
            GameEvents.OnPauseGameRequested -= PasueGame;
            GameEvents.OnExitGameRequested -= ExitGame;

            Instance = null;
            _whitePlayer = null;
            _blackPlayer = null;
        }
        void InitPlayersAndStart()
        {
            var setup = GameSetupManager.Instance;

            ChessColor player1Color = GameSettingsStore.CurrentSettings.HostColor;
            if (player1Color == ChessColor.White)
                AssignPlayers(setup.Player1, setup.Player2);
            else
                AssignPlayers(setup.Player2, setup.Player1);
            Debug.Log($"[GameManager.Start] player1 = {GameSetupManager.Instance?.Player1}");
            Debug.Log($"[GameManager.Start] player2 = {GameSetupManager.Instance?.Player2}");

            StartGame();
        }
        public void AssignPlayers(IPlayerController white, IPlayerController black)
        {
            _whitePlayer = white;
            _blackPlayer = black;
        }
        public void StartGame()
        {
            _blackPlayer.EndTurn();

            CurrentPlayer.StartTurn();
        }

        public void OnMoveCompleted()
        {
            CurrentPlayer.EndTurn();

            SwitchTurn();

            CurrentPlayer.StartTurn();
        }

        void SwitchTurn()
        {
            GameEvents.RequestChangeTurn();
            Debug.Log("Kolej gracza: " + GameStats.Instance.CurrentTurnColor);
        }
        public bool IsCurrentTurn(ChessColor color) => color == GameStats.Instance.CurrentTurnColor;
        void PasueGame()
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

        void ExitGame()
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
        public void MovePiece(string uci)
        {
            var(from,to) = UciHelper.ToBoardTile(uci);
            if (from.CurrentPiece == null) return;
            ChessPiece movingPiece = from.CurrentPiece;
            if(to.CurrentPiece != null)
            {
                BoardManager.Instance.allPieces.Remove(to.CurrentPiece);
                Destroy(to.CurrentPiece.gameObject);
            }
            from.SetPiece(null);
            to.SetPiece(movingPiece);
            movingPiece.CurrentTile = to;
            movingPiece.HasMoved = true;

            GameEvents.RequestClearHighlights();

            GameEvents.RequestRefreshBoardVisuals();

            OnMoveCompleted();
        }
    }
}
