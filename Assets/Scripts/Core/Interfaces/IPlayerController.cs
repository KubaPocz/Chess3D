using Core.Utilities;

namespace Core.Interfaces
{
    public interface IPlayerController
    {
        void StartTurn();
        void EndTurn();
        void Initialize(ChessColor playerColor);
    }
}
