using System.Collections.Generic;
using Core.Utilities;
using Game.Board;
using Game.Logic;
using UnityEngine;

namespace Game.Pieces
{
    abstract public class ChessPiece : MonoBehaviour
    {
        public bool HasMoved;
        public ChessColor Color { get; private set; }
        public BoardTile CurrentTile { get; set; }
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
            name = Color + "_" + GetComponent<ChessPiece>().PieceType;
            RotatePiece();
            ApplyColor();
            HasMoved = false;
        }

        protected void ApplyColor()
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material = (Color==ChessColor.White)?BoardManager.Instance.pieceWhite:BoardManager.Instance.pieceBlack;
        }
        void RotatePiece()
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
    }
}
