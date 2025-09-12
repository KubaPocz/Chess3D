using System.Collections;
using UnityEngine;

public class BotPlayerController : MonoBehaviour, IPlayerController
{
    public ChessColor PlayerColor { get; private set; }
    private StockfishEngine stockfish;
    public void StartTurn()
    {
        enabled = true;
        string fen = FENGenerator.GenerateFromBoard();
        string bestmove = stockfish.GetBestMove(fen);

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
        stockfish = new StockfishEngine();
        stockfish.StartEngine();
        stockfish.SetSkillLevel(GameConfigStore.CurrentConfig.Difficulty);
    }
    private IEnumerator ApplyMove(string uci)
    {
        yield return new WaitForSecondsRealtime(Random.Range(1f, 4f));
        GameManager.Instance.MovePiece(uci);
    }
}
