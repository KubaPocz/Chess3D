using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action<List<BoardTile>, ChessPiece> OnHighlightRequested;
    public static void RequestHighlights(List<BoardTile> tiles, ChessPiece piece)
    {
        OnHighlightRequested?.Invoke(tiles, piece);
    }
}
