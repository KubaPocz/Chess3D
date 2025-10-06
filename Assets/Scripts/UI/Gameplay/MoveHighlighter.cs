using System.Collections.Generic;
using Game;
using Game.Board;
using Game.Pieces;
using UnityEngine;

namespace UI.Gameplay
{
    public class MoveHighlighter : MonoBehaviour
    {
        public static MoveHighlighter Instance { get; private set; }
        [SerializeField] GameObject tileMoveHighlighter;
        [SerializeField] GameObject tileKillHighlighter;
        List<GameObject> _tileHighlighters = new ();

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        void OnEnable()
        {
            GameEvents.OnHighlightRequested += HighlightTiles;
            GameEvents.OnClearHighlightsRequested += ClearHighlights;
        }
        void OnDisable()
        {
            GameEvents.OnHighlightRequested -= HighlightTiles;
            GameEvents.OnClearHighlightsRequested -= ClearHighlights;
        }
        void HighlightTiles(List<BoardTile> tiles,ChessPiece piece)
        {
            ClearHighlights();
            foreach (BoardTile tile in tiles)
            {
                GameObject marker;
                if (tile.CurrentPiece != null && tile.CurrentPiece.Color != piece.Color)
                    marker = Instantiate(tileKillHighlighter, tile.transform.position+new Vector3(0,0.01f,0), Quaternion.identity);
                else
                    marker = Instantiate(tileMoveHighlighter, tile.transform.position, Quaternion.identity);
                marker.GetComponent<MoveMarker>().Init(tile, piece);
                _tileHighlighters.Add(marker);
            }
        }
        public void ClearHighlights()
        {
            foreach (GameObject marker in _tileHighlighters)
                Destroy(marker);

            _tileHighlighters.Clear();
        }
    }
}
