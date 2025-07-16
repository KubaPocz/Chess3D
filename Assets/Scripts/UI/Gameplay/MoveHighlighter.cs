using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MoveHighlighter : MonoBehaviour
{
    public static MoveHighlighter Instance { get; private set; }
    [SerializeField] private GameObject tileMoveHighlighter;
    [SerializeField] private GameObject tileKillHighlighter;
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
        GameEvents.OnHighlightRequested += HighlightTiles;
    }
    private void OnDisable()
    {
        GameEvents.OnHighlightRequested -= HighlightTiles;
    }
    private void HighlightTiles(List<BoardTile> tiles,ChessPiece piece)
    {
        ClearHighlights();
        selectedPiece = piece;
        foreach (BoardTile tile in tiles)
        {
            GameObject marker;
            if (tile.CurrentPiece != null && tile.CurrentPiece.Color != piece.Color)
                marker = Instantiate(tileKillHighlighter, tile.transform.position+new Vector3(0,0.01f,0), Quaternion.identity);
            else
                marker = Instantiate(tileMoveHighlighter, tile.transform.position, Quaternion.identity);
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
