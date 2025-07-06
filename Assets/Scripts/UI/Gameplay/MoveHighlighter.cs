using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MoveHighlighter : MonoBehaviour
{
    public static MoveHighlighter Instance { get; private set; }
    [SerializeField] private GameObject tileHighlighter;
    private List<GameObject> tileHighlighters = new ();
    private ChessPiece selectedPiece;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        ChessPiece.OnAnyPieceClicked += HighlightTiles;
    }
    private void OnDisable()
    {
        ChessPiece.OnAnyPieceClicked -= HighlightTiles;
    }
    private void HighlightTiles(List<BoardTile> tiles,ChessPiece piece)
    {
        ClearHighlights();
        selectedPiece = piece;
        foreach (BoardTile tile in tiles)
        {
            GameObject marker = Instantiate(tileHighlighter, tile.transform.position, Quaternion.identity);
            marker.GetComponent<MoveMarker>().Init(tile, piece);
            tileHighlighters.Add(marker);
        }
    }
    public void ClearHighlights()
    {
        foreach (GameObject marker in tileHighlighters)
            Destroy(marker);

        tileHighlighters.Clear();
    }
}
