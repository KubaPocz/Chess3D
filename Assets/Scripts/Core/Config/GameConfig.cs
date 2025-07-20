public class GameConfig
{
    public GameMode GameMode {  get; private set; }
    public ChessColor PlayerColor { get; private set; }
    public int Difficulty { get; private set; }

    public GameConfig(GameMode gameMode, ChessColor playerColor, int difficulty)
    {
        GameMode = gameMode;
        PlayerColor = playerColor;
        Difficulty = difficulty;
    }
}