using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<BoardTile> GetAvailableMoves()
    {
        List<BoardTile> moves = new();

        Vector2Int position = CurrentTile.GridPosition;

        Vector2Int[] directions = {
            new(2,1),
            new(2,-1),
            new(-2,1),
            new(-2,-1),
            new(1,2),
            new(-1,2),
            new(1,-2),
            new(-1,-2)
        };

        foreach (Vector2Int direction in directions)
        {
            int x = position.x + direction.x;
            int y = position.y + direction.y;

            if (!IsInsideBoard(x, y)) continue;

            if (IsEmpty(x, y) || IsEnemy(x, y))
                moves.Add(Board[x, y]);
        }

        return moves;
    }
}
