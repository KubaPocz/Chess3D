using UnityEngine;

public class BoardLoader : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private GameObject tilesParent;
    [SerializeField] private GameObject prefabTile;
    [Header("Pieces")]
    [SerializeField] private GameObject piecesParent;
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private GameObject rookPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject bishopPrefab;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject kingPrefab;
    [Header("BoardManager")]
    [SerializeField] private BoardManager BoardManager;

    private Material White;
    private Material Black;
    private BoardTile[,] tiles = new BoardTile[8, 8];

    void Start()
    {
        White = BoardManager.White;
        Black = BoardManager.Black;
        GenerateBoard();
        SpawnAllPieces();

        BoardManager.SetGameBoard(tiles);
    }

    void GenerateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(prefabTile, position, Quaternion.identity, tilesParent.transform);
                Renderer tile_Renderer = tile.GetComponent<Renderer>();
                BoardTile tile_boardTile = tile.GetComponent<BoardTile>();
                ChessColor color = (x + z) % 2 == 1 ? ChessColor.White : ChessColor.Black;
                Material material = (x + z) % 2 == 1 ? White : Black;
                tile_boardTile.Init(x, z, tile_Renderer, color, material);
                tiles[x,z] = tile_boardTile;
            }
        }
    }

    void SpawnAllPieces()
    {
        for(int x = 0; x < 8; x++)
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
        GameObject pieceObj = Instantiate(piecePrefab);
        ChessPiece piece = pieceObj.GetComponent<ChessPiece>();

        BoardTile tile = tiles[x,y];
        piece.Initialize(color, tile, tiles, piece.PieceType, BoardManager);
        tile.SetPiece(piece);

        BoardManager.allPieces.Add(piece);
    }
}
