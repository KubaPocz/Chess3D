using System.Collections.Generic;
using UnityEngine;

public class Bishop:ChessPiece
{
    public override void SetPieceType() => PieceType = PieceType.Bishop;
    public override List<BoardTile> GetAvailableMoves()
    {
        List<BoardTile> moves = new();

        Vector2Int position = CurrentTile.GridPosition;

        Vector2Int[] directions = {
            new(1,1),
            new(1,-1),
            new(-1,1),
            new(-1,-1)
        };

        foreach (Vector2Int direction in directions)
        {
            int x = position.x + direction.x;
            int y = position.y + direction.y;

            while (IsInsideBoard(x, y))
            {
                if (IsEmpty(x, y))
                    moves.Add(Board[x, y]);
                else if (IsEnemy(x, y))
                {
                    moves.Add(Board[x, y]);
                    break;
                }
                else
                    break;
                x += direction.x;
                y += direction.y;
            }

        }

        return moves;
    }
}
