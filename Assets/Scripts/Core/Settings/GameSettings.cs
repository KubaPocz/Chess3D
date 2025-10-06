using Core.Config;
using Core.Utilities;

namespace Core.Settings
{
    public class GameSettings
    {
        public GameMode GameMode {  get; private set; }
        public ChessColor HostColor;
        public ChessColor ClientColor;
        public int Difficulty { get; private set; }

        public GameSettings(GameMode gameMode, ChessColor hostColor, int difficulty)
        {
            GameMode = gameMode;
            HostColor = hostColor;
            ClientColor = HostColor==ChessColor.White ? ChessColor.Black : ChessColor.White;
            Difficulty = difficulty;
        }
        public GameSettings(GameMode gameMode, ChessColor hostColor)
        {
            GameMode = gameMode;
            HostColor = hostColor;
        }
    }
}