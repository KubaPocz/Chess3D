using System.Collections.Generic;
using UnityEngine;

public class King:ChessPiece
{
    public override List<BoardTile> GetAvailableMoves()
    {
        List<BoardTile> moves = new();

        Vector2Int position = CurrentTile.GridPosition;
        for (int x = CurrentTile.GridPosition.x - 1; x <= CurrentTile.GridPosition.x + 1; x++)
        {
            for (int y = CurrentTile.GridPosition.y - 1; y <= CurrentTile.GridPosition.y + 1; y++)
            {
                if (!IsInsideBoard(x, y)) continue;
                if ((x == position.x && y == position.y)) continue;
                if ((IsEmpty(x, y) || IsEnemy(x, y)) && !BoardManager.IsTileUnderAttack(Board[x, y],Color))
                    moves.Add(Board[x, y]);
            }
        }

        return moves;
    }
}
