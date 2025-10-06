using Core.Config;
using Core.Settings;
using Core.Utilities;
using UnityEngine;

namespace Game.Logic
{
    public class GameStats : MonoBehaviour
    {
        public static GameStats Instance;
        public int WhiteMoves {  get; private set; }
        public int BlackMoves { get; private set; }
        public ChessColor CurrentTurnColor {  get; set; }
        public float WhiteTime {  get; private set; }
        public float BlackTime {  get; private set; }


        void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            CurrentTurnColor = ChessColor.White;
            WhiteMoves = 0;
            BlackMoves = 0;
            if(GameSettingsStore.CurrentSettings.GameMode == GameMode.HumanVsHuman)
            {
                //do ewentualnej zmiany w ustawieniach gry w lobby
                WhiteTime = 15f;
                BlackTime = 15f;
            }
        }
        void Start()
        {
            GameEvents.OnChangeTurnRequested += ChangeCurrentTurn;
            GameEvents.OnAddPlayerMoveRequested += AddPlayerMoves;
        }
        void AddPlayerMoves()
        {
            if (CurrentTurnColor == ChessColor.White)
                AddWhiteMoves();
            else
                AddBlackMoves();
        }
        void AddWhiteMoves()
        {
            WhiteMoves++;
        }
        void AddBlackMoves()
        {
            BlackMoves++;
        }
        void ChangeCurrentTurn()
        {
            if (CurrentTurnColor == ChessColor.White)
                CurrentTurnColor = ChessColor.Black;
            else
                CurrentTurnColor = ChessColor.White;
        }
    }
}
