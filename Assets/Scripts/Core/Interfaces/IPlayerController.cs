using NUnit.Framework;
using System;
using UnityEngine;
public interface IPlayerController
{
    void StartTurn();
    void EndTurn();
    void Initialize(ChessColor playerColor);
}
