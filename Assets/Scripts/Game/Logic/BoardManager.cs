using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public GameManager GameManager;
    [SerializeField] public Material White;
    [SerializeField] public Material Black;
    public BoardTile[,] GameBoard { get; private set; }
    public List<ChessPiece> allPieces = new();
    public List<BoardTile> GetAllAttackedTiles(ChessColor byColor)
    {
        List<BoardTile> attackedTiles = new();
        foreach (ChessPiece piece in allPieces)
        {
            if (piece.Color == byColor) continue;
            attackedTiles.AddRange(piece.GetAvailableMoves(true));
        }
        return attackedTiles.Distinct().ToList();
    }
    public bool IsTileUnderAttack(BoardTile tile, ChessColor attackerColor) => GetAllAttackedTiles(attackerColor).Contains(tile);
    public void SetGameBoard(BoardTile[,] board)
    {
        GameBoard = board;
    }
}
