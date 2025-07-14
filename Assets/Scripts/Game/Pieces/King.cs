using System.Collections.Generic;
using UnityEngine;

public class King:ChessPiece
{
    public override void SetPieceType() => PieceType = PieceType.King;
    public override List<BoardTile> GetAvailableMoves(bool includeIllegal = false)
    {
        List<BoardTile> moves = new();

        Vector2Int position = CurrentTile.GridPosition;
        for (int x = position.x - 1; x <= position.x + 1; x++)
        {
            for (int y = position.y - 1; y <= position.y + 1; y++)
            {
                if (!IsInsideBoard(x, y)) continue;
                if (x == position.x && y == position.y) continue;

                if (IsEmpty(x, y) || IsEnemy(x, y))
                {
                    if (includeIllegal || !BoardManager.Instance.IsTileUnderAttack(Board[x, y], Color))
                        moves.Add(Board[x, y]);
                }
            }
        }

        return moves;
    }
}
