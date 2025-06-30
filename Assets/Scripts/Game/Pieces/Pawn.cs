using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override void SetPieceType() => PieceType = PieceType.Pawn;
    public override List<BoardTile> GetAvailableMoves()
    {
        List<BoardTile> moves = new();
        Vector2Int position = CurrentTile.GridPosition;
        int direction = (Color == ChessColor.White) ? 1 : -1;
        int startRow = (Color == ChessColor.White) ? 1 : 6;
        int nextRow = position.y + direction;

        if(IsEmpty(position.x, nextRow))
            moves.Add(Board[position.x, nextRow]);

        if (position.y == startRow && IsEmpty(position.x, nextRow) && IsEmpty(position.y, position.y + 2 * direction))
            moves.Add(Board[position.x, position.y + 2 * direction]);

        for(int dx = -1; dx <= 1; dx += 2)
        {
            int targetX = position.x + dx;
            if (IsEnemy(targetX, nextRow))
                moves.Add(Board[targetX, nextRow]);
        }
        return moves;
    }
}
