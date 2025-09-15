using UnityEngine;

public class MoveMarker : MonoBehaviour
{
    private BoardTile currentTile;
    private ChessPiece selectedPiece;
    public void Init(BoardTile tile,ChessPiece piece)
    {
        currentTile = tile;
        selectedPiece = piece;
    }
    private void OnMouseDown()
    {
        Debug.Log(selectedPiece);
        if (selectedPiece != null)
        {
            GameEvents.RequestMovePiece(selectedPiece.CurrentTile, currentTile);   
        }
    }
}
