using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public static event Action<List<BoardTile>, ChessPiece> OnHighlightRequested;
    public static event Action<Animator,Animator> OnHidePanelRequested;
    public static void RequestHighlights(List<BoardTile> tiles, ChessPiece piece)
    {
        OnHighlightRequested?.Invoke(tiles, piece);
    }
    public static void RequestHidePanel(Animator panelHide,Animator panelShow)
    {
        OnHidePanelRequested?.Invoke(panelHide, panelShow);
    }
}
