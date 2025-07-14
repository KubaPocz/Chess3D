using UnityEngine;
using static Codice.CM.Common.CmCallContext;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private IPlayerController whitePlayer;
    private IPlayerController blackPlayer;

    public IPlayerController CurrentPlayer;
    public ChessColor CurrentTurnColor { get; private set; }
    public IPlayerController WaitingPlayer => (CurrentPlayer == whitePlayer) ? blackPlayer : whitePlayer;
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        var setup = FindAnyObjectByType<GameSetupManager>();
        AssignPlayers(setup.player1, setup.player2);
        StartGame();
    }
    public void AssignPlayers(IPlayerController white, IPlayerController black)
    {
        whitePlayer = white;
        blackPlayer = black;
    }
    public void StartGame()
    {
        CurrentPlayer = whitePlayer;
        blackPlayer.EndTurn();
        CurrentTurnColor = ChessColor.White;

        CurrentPlayer.StartTurn();
    }

    public void OnMoveCompleted()
    {
        CurrentPlayer.EndTurn();

        SwitchTurn();

        CurrentPlayer.StartTurn();
    }

    private void SwitchTurn()
    {
        CurrentPlayer = (CurrentPlayer == whitePlayer) ? blackPlayer : whitePlayer;
        CurrentTurnColor = (CurrentTurnColor==ChessColor.White)? ChessColor.Black: ChessColor.White;
        Debug.Log("Kolej gracza: "+ CurrentTurnColor);
    }

    public bool IsCurrentTurn(ChessColor color) => color == CurrentTurnColor;
}
