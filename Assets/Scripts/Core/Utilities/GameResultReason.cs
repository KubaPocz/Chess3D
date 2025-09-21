namespace Core.Utilities
{
    public enum GameResultReason
    {
        Checkmate,              // Szach-mat
        Stalemate,              // Pat
        Resignation,            // Poddanie
        Timeout,                // Przekroczenie czasu
        ThreefoldRepetition,    // Trzykrotne powtorzenie pozycji
        FiftyMoveRule,          // Regula 50 posuniec
        InsufficientMaterial,   // Niewystarczajacy material do mata
        Agreement               // Remis za obopolna zgoda
    }
}
