using UnityEngine;

public class BoardLoader : MonoBehaviour
{
    [SerializeField] private GameObject boardParent;
    [SerializeField] private GameObject prefabTile;
    [SerializeField] private Material white;
    [SerializeField] private Material black;

    BoardTile[,] tiles = new BoardTile[8, 8];

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject tile = Instantiate(prefabTile, position, Quaternion.identity, boardParent.transform);
                Renderer tile_Renderer = tile.GetComponent<Renderer>();
                BoardTile tile_boardTile = tile.GetComponent<BoardTile>();
                ChessColor color = (x + z) % 2 == 0 ? ChessColor.White : ChessColor.Black;
                Material material = (x + z) % 2 == 0 ? white : black;
                tile_boardTile.Init(x, z, tile_Renderer, color, material);
                tiles[x,z] = tile_boardTile;
            }
        }
    }
}
