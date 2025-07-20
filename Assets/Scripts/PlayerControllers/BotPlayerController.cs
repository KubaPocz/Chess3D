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
            ApplyMove(bestmove);
    }
    public void EndTurn()
    {
        enabled = false;
    }
    public void Initialize(ChessColor playerColor)
    {
        PlayerColor = playerColor;
        stockfish = new StockfishEngine();
        stockfish.StartEngine();
        stockfish.SetSkillLevel(GameConfigStore.CurrentConfig.Difficulty);
    }
    private void ApplyMove(string uci)
    {
        var from = UCIHelper.ToBoardTile(uci.Substring(0, 2));
        var to = UCIHelper.ToBoardTile(uci.Substring(2, 2));

        var piece = from.CurrentPiece;
        piece?.MovePiece(to);
    }
}
