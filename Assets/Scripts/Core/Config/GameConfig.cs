public class GameConfig
{
    public GameMode GameMode {  get; private set; }
    public ChessColor PlayerColor { get; private set; }

    public GameConfig(GameMode gameMode, ChessColor playerColor)
    {
        GameMode = gameMode;
        PlayerColor = playerColor;
    }
}