using NUnit.Framework;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;

public class HumanPlayerController : MonoBehaviour, IPlayerController
{
    public static event Action<List<BoardTile>,ChessPiece> HighlightTiles;
    public ChessColor PlayerColor { get; private set; }
    public void StartTurn()
    {
        enabled = true;
    }
    public void EndTurn()
    {
        enabled = false;
    }
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
    private void OnPieceSelected(List<BoardTile> tiles ,ChessPiece piece)
    {
        GameEvents.RequestHighlights(tiles, piece);
    }
}
