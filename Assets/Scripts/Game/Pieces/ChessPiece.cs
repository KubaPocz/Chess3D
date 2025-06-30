using System.Collections.Generic;
using UnityEngine;

abstract public class ChessPiece : MonoBehaviour
{
    public ChessColor Color { get; private set; }
    public BoardTile CurrentTile { get; private set; }
    protected BoardTile[,] Board;
    public PieceType PieceType { get; private set; }
    public BoardManager BoardManager { get; private set; }
    public void Initialize(ChessColor color, BoardTile startTile, BoardTile[,] board, PieceType pieceType, BoardManager boardManager)
    {
        Color = color;
        CurrentTile = startTile;
        Board = board;
        transform.position = startTile.transform.position;
        PieceType = pieceType;
        BoardManager = boardManager;
        RotatePiece();
        ApplyColor();
    }

    protected void ApplyColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = (Color==ChessColor.White)?BoardManager.White:BoardManager.Black;
    }
    private void RotatePiece()
    {
        if(Color==ChessColor.Black)
            transform.rotation *= Quaternion.Euler(0,180,0);
    }
    public abstract List<BoardTile> GetAvailableMoves();
    protected bool IsInsideBoard(int x, int z) => x >= 0 && z >= 0 && x < 8 && z < 8;
    protected bool IsEmpty(int x, int z)
    {
        if (!IsInsideBoard(x, z)) return false;
        return Board[x, z].CurrentPiece == null;
    }
    protected bool IsEnemy(int x, int z)
    {
        if (!IsInsideBoard(x, z)) return false;
        if (Board[x, z].CurrentPiece == null) return false;
        ChessPiece piece = Board[x, z].CurrentPiece;
        return piece != null && piece.Color != this.Color;
    }
    protected bool IsAlly(int x, int z)
    {
        if(!IsInsideBoard(x, z)) return false;

        ChessPiece piece = Board[x, z].CurrentPiece;
        return piece!= null && piece.Color == this.Color;
    }
}
