public enum GameResultReason
{
    Checkmate,              // Szach-mat
    Stalemate,              // Pat
    Resignation,            // Poddanie
    Timeout,                // Przekroczenie czasu
    ThreefoldRepetition,    // Trzykrotne powtórzenie pozycji
    FiftyMoveRule,          // Regu³a 50 posuniêæ
    InsufficientMaterial,   // Niewystarczaj¹cy materia³ do mata
    Agreement               // Remis za obopóln¹ zgod¹
}
