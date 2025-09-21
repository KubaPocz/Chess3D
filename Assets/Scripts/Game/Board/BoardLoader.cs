using Core.Utilities;
using Game.Logic;
using Game.Pieces;
using UnityEngine;

namespace Game.Board
{
    public class BoardLoader : MonoBehaviour
    {
        [Header("Board frame")]
        [SerializeField] GameObject boardFrame;

        [Header("Tiles")]
        [SerializeField] GameObject tilesParent;
        [SerializeField] GameObject prefabTile;

        [Header("Pieces")]
        [SerializeField] GameObject piecesParent;
        [SerializeField] GameObject pawnPrefab;
        [SerializeField] GameObject rookPrefab;
        [SerializeField] GameObject knightPrefab;
        [SerializeField] GameObject bishopPrefab;
        [SerializeField] GameObject queenPrefab;
        [SerializeField] GameObject kingPrefab;

        Material _tileWhite;
        Material _tileBlack;
        BoardTile[,] _tiles = new BoardTile[8, 8];

        void Start()
        {
            _tileWhite = BoardManager.Instance.tileWhite;
            _tileBlack = BoardManager.Instance.tileBlack;
            GenerateBoard();
            SpawnAllPieces();
            BoardManager.Instance.SetGameBoard(_tiles);
        }
        void GenerateBoard()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int z = 0; z < 8; z++)
                {
                    Vector3 position = new Vector3(x, 0, z);
                    GameObject tile = Instantiate(prefabTile, position, Quaternion.identity, tilesParent.transform);
                    var tileRenderer = tile.GetComponent<Renderer>();
                    var tileBoardTile = tile.GetComponent<BoardTile>();
                    ChessColor color = (x + z) % 2 == 1 ? ChessColor.White : ChessColor.Black;
                    Material material = (x + z) % 2 == 1 ? _tileWhite : _tileBlack;
                    tileBoardTile.Init(x, z, tileRenderer, color, material);
                    _tiles[x,z] = tileBoardTile;
                }
            }
        }

        void SpawnAllPieces()
        {
            for (int x = 0; x < 8; x++)
            {
                SpawnPiece(pawnPrefab, ChessColor.White, x, 1);
                SpawnPiece(pawnPrefab, ChessColor.Black, x, 6);
            }
            //Rook's
            SpawnPiece(rookPrefab, ChessColor.White, 0, 0);
            SpawnPiece(rookPrefab, ChessColor.White, 7, 0);

            SpawnPiece(rookPrefab, ChessColor.Black, 0, 7);
            SpawnPiece(rookPrefab, ChessColor.Black, 7, 7);

            //Knight's
            SpawnPiece(knightPrefab, ChessColor.White, 1, 0);
            SpawnPiece(knightPrefab, ChessColor.White, 6, 0);

            SpawnPiece(knightPrefab, ChessColor.Black, 1, 7);
            SpawnPiece(knightPrefab, ChessColor.Black, 6, 7);

            //Bishop's
            SpawnPiece(bishopPrefab, ChessColor.White, 2, 0);
            SpawnPiece(bishopPrefab, ChessColor.White, 5, 0);

            SpawnPiece(bishopPrefab, ChessColor.Black, 2, 7);
            SpawnPiece(bishopPrefab, ChessColor.Black, 5, 7);

            //Queen's
            SpawnPiece(queenPrefab, ChessColor.White, 3, 0);
            SpawnPiece(queenPrefab, ChessColor.Black, 3, 7);


            //King's
            SpawnPiece(kingPrefab, ChessColor.White, 4, 0);
            SpawnPiece(kingPrefab, ChessColor.Black, 4, 7);

        }
        void SpawnPiece(GameObject piecePrefab,ChessColor color, int x, int y)
        {
            GameObject pieceObj = Instantiate(piecePrefab,piecesParent.transform);
            ChessPiece piece = pieceObj.GetComponent<ChessPiece>();

            BoardTile tile = _tiles[x,y];
            piece.Initialize(color, tile, _tiles);
            tile.SetPiece(piece);

            BoardManager.Instance.allPieces.Add(piece);
        }
    }
}
