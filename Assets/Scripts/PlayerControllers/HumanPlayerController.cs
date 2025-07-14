using UnityEngine;

public class HumanPlayerController : MonoBehaviour, IPlayerController
{
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
}
