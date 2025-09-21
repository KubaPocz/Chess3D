using NUnit.Framework;
using System;
using System.Collections.Generic;
using Core.Boot;
using Core.Config;
using Core.Utilities;
using Game.Board;
using Game.Pieces;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameEvents
{
    public static event Action<List<BoardTile>, ChessPiece> OnHighlightRequested;
    public static event Action OnClearHighlightsRequested;
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
    public static event Action OnStartGameOnlineRequested;

    public static event Action CreateLobbyRequested;
    public static event Action<string> JoinLobbyByCodeRequested;
    public static event Action LeaveOrDeleteLobbyRequested;
    // offline or online
    public static event Action<string> OnMovePieceOfflineRequested;
    public static event Action<string> OnMovePieceOnlineRequested;

    public static event Action OnRefreshBoardVisualsOffline;
    public static event Action OnRefreshBoardVisualsOnline;
    // Powiadomienia do UI:
    public static event Action<string, string> LobbyCreated;
    public static event Action<string, string> LobbyJoined;
    public static event Action LobbyLeftOrDeleted;
    public static event Action<string> OnNotifyError;
    public static event Action<string, string> LobbyPlayersUpdated;
    public static event Action<List<string>> OnPlayersListUpdated;
    public static event Action LobbyClosedByHost;
    public static event Action OnSwapTeamsRequested;
    public static event System.Func<int, System.Threading.Tasks.Task<string>> HostAllocateRelayRequested;

    // Klient prosi o JoinRelay + StartClient (scena zsynchronizuje si� po po��czeniu)
    public static event System.Func<string, System.Threading.Tasks.Task> ClientJoinRelayRequested;

    // Helpery do wywo�a� (opcjonalne, dla czytelno�ci)
    public static System.Threading.Tasks.Task<string> RequestHostAllocateRelayAsync(int expectedClients)
        => HostAllocateRelayRequested != null ? HostAllocateRelayRequested.Invoke(expectedClients)
                                              : System.Threading.Tasks.Task.FromResult<string>(null);

    public static System.Threading.Tasks.Task RequestClientJoinRelayAsync(string joinCode)
        => ClientJoinRelayRequested != null ? ClientJoinRelayRequested.Invoke(joinCode)
                                            : System.Threading.Tasks.Task.CompletedTask;


    public static void RequestHighlights(List<BoardTile> tiles, ChessPiece piece) => OnHighlightRequested?.Invoke(tiles, piece);
    public static void RequestClearHighlights() => OnClearHighlightsRequested?.Invoke();
    public static void RequestHidePanel(Animator panelHide,Animator panelShow) => OnHidePanelRequested?.Invoke(panelHide, panelShow);
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
    public static void RequestStartGameOnline()
    {
        SceneLoader.SceneToLoad = "GameBoard";
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
        OnStartGameOnlineRequested?.Invoke();
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
    public static void RequestCreateLobby() => CreateLobbyRequested?.Invoke();
    public static void RequestJoinByCode(string code) => JoinLobbyByCodeRequested?.Invoke(code);
    public static void RequestLeaveOrDelete() => LeaveOrDeleteLobbyRequested?.Invoke();

    public static void NotifyCreated(string id, string code) => LobbyCreated?.Invoke(id, code);
    public static void NotifyJoined(string id, string code) => LobbyJoined?.Invoke(id, code);
    public static void NotifyLeftOrDeleted() => LobbyLeftOrDeleted?.Invoke();
    public static void NotifyError(string msg) => OnNotifyError?.Invoke(msg);
    public static void NotifyPlayersUpdated(string hostName, string guestName)
        => LobbyPlayersUpdated?.Invoke(hostName, guestName);
    public static void NotifyPlayersListUpdated(List<string> players)
    => OnPlayersListUpdated?.Invoke(players);
    public static void NotifyLobbyClosedByHost() => LobbyClosedByHost?.Invoke();
    public static void RequestSwapTeams() => OnSwapTeamsRequested?.Invoke();
    public static void RequestMovePiece(string uci)
    {
        if (GameConfigStore.CurrentConfig.GameMode == GameMode.HumanVsHuman)
            OnMovePieceOnlineRequested?.Invoke(uci);
        else
            OnMovePieceOfflineRequested?.Invoke(uci);
    }
    public static void RequestRefreshBoardVisuals()
    {
        if (GameConfigStore.CurrentConfig.GameMode == GameMode.HumanVsHuman)
            OnRefreshBoardVisualsOnline?.Invoke();
        else
            OnRefreshBoardVisualsOffline?.Invoke();
    }
}
