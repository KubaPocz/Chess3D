using System.Collections;
using AI;
using Core.Config;
using Core.Interfaces;
using Core.Utilities;
using Game.Logic;
using UnityEngine;

namespace PlayerControllers
{
    public class BotPlayerController : MonoBehaviour, IPlayerController
    {
        public ChessColor PlayerColor { get; private set; }
        StockfishEngine _stockfish;
        public void StartTurn()
        {
            enabled = true;
            string fen = FenGenerator.GenerateFromBoard();
            string bestmove = _stockfish.GetBestMove(fen);

            if (bestmove != null)
                StartCoroutine(ApplyMove(bestmove));
        }
        public void EndTurn()
        {
            GameEvents.RequestAddPlayerMove();
            enabled = false;
        }
        public void Initialize(ChessColor playerColor)
        {
            PlayerColor = playerColor;
            _stockfish = new StockfishEngine();
            _stockfish.StartEngine();
            _stockfish.SetSkillLevel(GameConfigStore.CurrentConfig.Difficulty);
        }
        IEnumerator ApplyMove(string uci)
        {
            yield return new WaitForSecondsRealtime(Random.Range(1f, 4f));
            GameManager.Instance.MovePiece(uci);
        }
    }
}
