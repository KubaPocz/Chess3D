using System.Collections.Generic;
using Core.Interfaces;
using Core.Utilities;
using Game;
using Game.Board;
using Game.Boot;
using Game.Pieces;
using UnityEngine;

namespace Networking
{
    public class NetworkPlayerController : MonoBehaviour,IPlayerController
    {
        public ChessColor PlayerColor;

        public void Start()
        {
            GameSetupManager.Instance.RegisterPlayer(this);
        }
        public void Initialize(ChessColor playerColor)
        {
            PlayerColor = playerColor;
            Instantiate(PlayerColor == ChessColor.White
                ? OnlineSessionCoordinator.Instance.gameConfig.whiteCamera
                : OnlineSessionCoordinator.Instance.gameConfig.blackCamera);
            if (PlayerColor == ChessColor.Black)
                gameObject.SetActive(false);
        }
        void OnEnable()
        {
            ChessPiece.OnAnyPieceClicked += OnPieceSelected;
        }
        void OnDisable()
        {
            ChessPiece.OnAnyPieceClicked -= OnPieceSelected;
        }
        public void StartTurn()
        {
            enabled = true;
        }
        public void EndTurn()
        {
            GameEvents.RequestAddPlayerMove();
            enabled = false;
        }
        void OnPieceSelected(List<BoardTile> tiles, ChessPiece piece)
        {
            GameEvents.RequestHighlights(tiles, piece);
        }
    }
}
