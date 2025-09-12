using UnityEngine;

public static class UCIHelper
{
    public static (BoardTile from, BoardTile to) ToBoardTile(string uci)
    {
        int fromFile = uci[0] - 'a';
        int fromRank = int.Parse(uci[1].ToString()) - 1;
        BoardTile fromTile = BoardManager.Instance.GameBoard[fromFile, fromRank];

        int toFile = uci[2] - 'a';
        int toRank = int.Parse(uci[3].ToString()) - 1;
        BoardTile toTile = BoardManager.Instance.GameBoard[toFile, toRank];

        return (fromTile, toTile);
    }
    public static string ToUCI(BoardTile from, BoardTile to)
    {
        return (from.TileName + to.TileName).ToLower();
    }

}
