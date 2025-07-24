using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameEvents
{
    public static event Action<List<BoardTile>, ChessPiece> OnHighlightRequested;
    public static event Action<Animator,Animator> OnHidePanelRequested;
    public static event Action<ChessColor> OnColorChangeRequested;
    public static event Action<int> OnGameDifficultyChangeRequested;
    public static event Action OnChangeTurnRequested;
    public static event Action OnAddPlayerMoveRequested;
    public static event Action OnPauseGameRequested;
    public static event Action OnRestartGameRequested;
    public static event Action OnSurrenderGameRequested;
    public static event Action OnExitGameRequested;
    public static event Action<GameResult,GameResultReason> OnGameEnds;
    //public static event Action<ChessColor,float> OnStartGameOfflineRequested;
    
    public static void RequestHighlights(List<BoardTile> tiles, ChessPiece piece)
    {
        OnHighlightRequested?.Invoke(tiles, piece);
    }
    public static void RequestHidePanel(Animator panelHide,Animator panelShow)
    {
        OnHidePanelRequested?.Invoke(panelHide, panelShow);
    }
    public static void RequestColorChange(ChessColor color)
    {
        OnColorChangeRequested?.Invoke(color);
        Debug.Log($"selected color: {color}");
    }
    public static void RequestStartGameOffline(ChessColor playerColor, int difficulty)
    {
        GameConfigStore.CurrentConfig = new GameConfig(GameMode.HumanVsBot, playerColor,difficulty);
        //OnStartGameOfflineRequested?.Invoke(playerColor, difficulty);
        SceneLoader.SceneToLoad = "GameBoard";
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }
    public static void RequestChangeGameDifficulty(int difficulty)
    {
        OnGameDifficultyChangeRequested?.Invoke(difficulty);
    }
    public static void RequestChangeTurn()
    {
        OnChangeTurnRequested?.Invoke();
    }
    public static void RequestAddPlayerMove()
    {
        OnAddPlayerMoveRequested?.Invoke();
    }
    public static void RequestPauseGame()
    {
        OnPauseGameRequested?.Invoke();
    }
    public static void RequestRestartGame()
    {
        OnRestartGameRequested?.Invoke();
    }
    public static void RequestSurrenderGame()
    {
        //OnSurrenderGameRequested?.Invoke();
        OnExitGameRequested?.Invoke();
    }
    public static void RequestExitGame()
    {
        OnExitGameRequested?.Invoke();
    }
    public static void RequestEndGame(GameResult gameResult, GameResultReason gameResultReason)
    {
        OnGameEnds?.Invoke(gameResult,gameResultReason);
    }
}
