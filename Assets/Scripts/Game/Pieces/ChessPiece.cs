using System.Collections.Generic;
using UnityEngine;

abstract public class ChessPiece : MonoBehaviour
{
    public bool HasMoved = false;
    public ChessColor Color { get; private set; }
    public BoardTile CurrentTile { get; private set; }
    protected BoardTile[,] Board;
    public PieceType PieceType { get; protected set; }
    public static event System.Action<List<BoardTile>,ChessPiece> OnAnyPieceClicked;
    public void Initialize(ChessColor color, BoardTile startTile, BoardTile[,] board)
    {
        Color = color;
        CurrentTile = startTile;
        Board = board;
        transform.position = startTile.transform.position;
        SetPieceType();
        name = Color.ToString() + "_" + GetComponent<ChessPiece>().PieceType.ToString();
        RotatePiece();
        ApplyColor();
    }

    protected void ApplyColor()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = (Color==ChessColor.White)?BoardManager.Instance.pieceWhite:BoardManager.Instance.pieceBlack;
    }
    private void RotatePiece()
    {
        if(Color==ChessColor.Black)
            transform.rotation *= Quaternion.Euler(0,180,0);
    }
    public abstract List<BoardTile> GetAvailableMoves(bool includeIllegal = false);
    public abstract void SetPieceType();
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
    public void OnMouseDown()
    {
        if (!GameManager.Instance.IsCurrentTurn(Color))
            return;
        OnAnyPieceClicked?.Invoke(GetAvailableMoves(), this);
    }
    public void MovePiece(BoardTile targetTile)
    {
        if (CurrentTile != null)
        {
            CurrentTile.SetPiece(null);
        }

        if(targetTile.CurrentPiece != null)
        {
            ChessPiece target = targetTile.CurrentPiece;
            BoardManager.Instance.allPieces.Remove(target);
            Destroy(target.gameObject);
        }

        targetTile.SetPiece(this);
        CurrentTile = targetTile;

        transform.position = targetTile.transform.position;

        HasMoved = true;

        GameManager.Instance.OnMoveCompleted();
    }
}
