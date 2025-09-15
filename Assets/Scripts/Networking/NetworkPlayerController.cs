using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerController : MonoBehaviour, IPlayerController
{
    public static event Action<List<BoardTile>, ChessPiece> HighlightTiles;
    public ChessColor PlayerColor;
    public void Initialize(ChessColor playerColor)
    {
        PlayerColor = playerColor;
    }
    private void OnEnable()
    {
        ChessPiece.OnAnyPieceClicked += OnPieceSelected;
    }
    private void OnDisable()
    {
        ChessPiece.OnAnyPieceClicked -= OnPieceSelected;
    }
    public void StartTurn()
    {
        enabled = true;
    }
    public void EndTurn()
    {
        GameEvents.RequestAddPlayerMove();
        enabled = false;
    }
    private void OnPieceSelected(List<BoardTile> tiles, ChessPiece piece)
    {
        GameEvents.RequestHighlights(tiles, piece);
    }
}
