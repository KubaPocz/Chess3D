using System;
using System.Collections.Generic;
using Core.Config;
using Core.Interfaces;
using Core.Utilities;
using Game.Board;
using Game.Boot;
using Game.Pieces;
using Unity.Netcode;

namespace Networking
{
    public class NetworkPlayerController : NetworkBehaviour, IPlayerController
    {
        public NetworkVariable<ChessColor> PlayerColor = new NetworkVariable<ChessColor>();
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                if (OwnerClientId == NetworkManager.ServerClientId)
                {
                    PlayerColor.Value = GameConfigStore.CurrentConfig.PlayerColor;
                }
                else
                {
                    PlayerColor.Value = GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White
                        ? ChessColor.Black
                        : ChessColor.White;
                }
            }

            GameSetupManager.Instance.RegisterPlayer(this);
        }
        public void Initialize(ChessColor playerColor)
        {
            PlayerColor.Value = playerColor;
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
