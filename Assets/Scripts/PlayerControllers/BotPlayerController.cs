using UnityEngine;

public class BotPlayerController : MonoBehaviour, IPlayerController
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
