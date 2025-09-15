public class GameConfig
{
    public GameMode GameMode {  get; private set; }
    public ChessColor PlayerColor { get;  set; }
    public int Difficulty { get; private set; }

    public GameConfig(GameMode gameMode, ChessColor playerColor, int difficulty)
    {
        GameMode = gameMode;
        PlayerColor = playerColor;
        Difficulty = difficulty;
    }
    public GameConfig(GameMode gameMode, ChessColor hostColor)
    {
        GameMode = gameMode;
        PlayerColor = hostColor;
    }
}