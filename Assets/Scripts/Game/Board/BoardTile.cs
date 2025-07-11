using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BoardTile : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }
    public ChessColor Color { get; private set; }
    public string TileName { get; private set; }
    public ChessPiece CurrentPiece { get; private set; }
    public void Init(int x, int z, Renderer renderer, ChessColor color, Material material)
    {
        GridPosition = new Vector2Int(x, z);
        Color = color;

        char file = (char)('A' + x);
        int rank = z + 1;

        TileName = $"{file}{rank}";
        CurrentPiece = null;

        gameObject.name = $"Tile_{TileName}";

        renderer.material = material;
    }
    public void SetPiece(ChessPiece piece)
    {
        CurrentPiece = piece;
    }
}
