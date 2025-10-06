using System.Collections.Generic;
using Core.Boot;
using Core.Interfaces;
using Core.Utilities;
using Game;
using Game.Board;
using Game.Pieces;
using UnityEngine;

namespace PlayerControllers
{
    public class HumanPlayerController : MonoBehaviour, IPlayerController
    {
        public ChessColor PlayerColor;
        [SerializeField] GameConfig gameConfig;
        public void StartTurn()
        {
            enabled = true;
        }
        public void EndTurn()
        {
            GameEvents.RequestAddPlayerMove();
            enabled = false;
        }
        public void Initialize(ChessColor playerColor)
        {
            PlayerColor = playerColor;
            Instantiate(PlayerColor == ChessColor.White ? gameConfig.whiteCamera : gameConfig.blackCamera);
        }
        void OnEnable()
        {
            ChessPiece.OnAnyPieceClicked += OnPieceSelected;
        }
        void OnDisable()
        {
            ChessPiece.OnAnyPieceClicked -= OnPieceSelected;
        }
        void OnPieceSelected(List<BoardTile> tiles ,ChessPiece piece)
        {
            GameEvents.RequestHighlights(tiles, piece);
        }
    }
}
