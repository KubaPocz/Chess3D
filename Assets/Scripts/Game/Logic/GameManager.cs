using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IPlayerController whitePlayer;
    private IPlayerController blackPlayer;

    public void StartGame(IPlayerController white, IPlayerController black)
    {
        whitePlayer = white;
        blackPlayer = black;
        whitePlayer.StartTurn();
    }

    public void OnMoveCompleted()
    {

    }
}
