using UnityEngine;
public interface IPlayerController
{
    void StartTurn();
    void EndTurn();
    void Initialize(ChessColor playerColor);
}
