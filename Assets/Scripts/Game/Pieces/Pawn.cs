using System.Collections.Generic;
using Core.Utilities;
using Game.Board;
using UnityEngine;

namespace Game.Pieces
{
    public class Pawn : ChessPiece
    {
        public override void SetPieceType() => PieceType = PieceType.Pawn;
        public override List<BoardTile> GetAvailableMoves(bool includeIllegal = false)
        {
            List<BoardTile> moves = new();
            Vector2Int position = CurrentTile.GridPosition;
            int direction = (Color == ChessColor.White) ? 1 : -1;
            int startRow = (Color == ChessColor.White) ? 1 : 6;
            int nextRow = position.y + direction;

            if(IsEmpty(position.x, nextRow) && !includeIllegal)
                moves.Add(Board[position.x, nextRow]);

            if (!HasMoved && position.y == startRow && IsEmpty(position.x, nextRow) && IsEmpty(position.x, position.y + 2 * direction))
                moves.Add(Board[position.x, position.y + 2 * direction]);

            for(int dx = -1; dx <= 1; dx += 2)
            {
                int targetX = position.x + dx;
                if (IsInsideBoard(targetX,nextRow)&& (IsEnemy(targetX, nextRow) || includeIllegal))
                    moves.Add(Board[targetX, nextRow]);
            }
            return moves;
        }
    }
}
