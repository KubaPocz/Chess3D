using UnityEngine;

public static class UCIHelper
{
    public static BoardTile ToBoardTile(string uci)
    {
        int file = uci[0] - 'a';
        int rank = int.Parse(uci[1].ToString()) - 1;

        return BoardManager.Instance.GameBoard[file, rank];
    }
}
