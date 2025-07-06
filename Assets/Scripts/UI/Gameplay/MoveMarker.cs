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
        if (selectedPiece != null)
        {
            selectedPiece.MovePiece(currentTile);
            MoveHighlighter.Instance.ClearHighlights();
        }
    }
}
