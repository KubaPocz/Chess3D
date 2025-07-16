using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] public Material tileWhite;
    [SerializeField] public Material tileBlack;

    [SerializeField] public Material pieceWhite;
    [SerializeField] public Material pieceBlack;
    public BoardTile[,] GameBoard { get; private set; }
    [NonSerialized] public List<ChessPiece> allPieces = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
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
