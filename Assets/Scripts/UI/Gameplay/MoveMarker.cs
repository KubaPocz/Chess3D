using Game.Board;
using Game.Logic;
using Game.Pieces;
using UnityEngine;

namespace UI.Gameplay
{
    public class MoveMarker : MonoBehaviour
    {
        BoardTile currentTile;
        ChessPiece selectedPiece;
        public void Init(BoardTile tile,ChessPiece piece)
        {
            currentTile = tile;
            selectedPiece = piece;
        }
        void OnMouseDown()
        {
            Debug.Log(selectedPiece);
            if (selectedPiece != null)
            {
                GameEvents.RequestMovePiece(UciHelper.ToUci(selectedPiece.CurrentTile, currentTile));   
            }
        }
    }
}
