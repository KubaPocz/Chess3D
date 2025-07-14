using UnityEngine;

public static class FENGenerator
{
    public static string GenerateFromBoard()
    {
        var board = BoardManager.Instance.GameBoard;
        string fen = "";
        for (int rank = 7; rank >=0; rank--)
        {
            int emptyCount = 0;
            for(int file = 0; file <8;file++)
            {
                var tile = board[file, rank];
                var piece = tile.CurrentPiece;

                if (piece == null)
                {
                    emptyCount++;
                    continue;
                }
                if(emptyCount>0)
                {
                    fen += emptyCount;
                    emptyCount = 0;
                }
                fen += GetFENSymbol(piece);
            }
            if(emptyCount > 0)
                fen += emptyCount;
            if (rank > 0)
                fen += "/";
        }
        string turn = GameManager.Instance.CurrentTurnColor == ChessColor.White ? "w" : "b";
        fen += $" {turn} - - 0 1";
        return fen;
    }
    private static string GetFENSymbol(ChessPiece piece)
    {
        string letter = piece.PieceType switch
        {
            PieceType.King => "k",
            PieceType.Queen => "q",
            PieceType.Rook => "r",
            PieceType.Bishop => "b",
            PieceType.Knight => "n",
            PieceType.Pawn => "p",
            _ => "?"
        };

        return piece.Color == ChessColor.White?letter.ToUpper() : letter;
    }
}
