public enum GameResultReason
{
    Checkmate,              // Szach-mat
    Stalemate,              // Pat
    Resignation,            // Poddanie
    Timeout,                // Przekroczenie czasu
    ThreefoldRepetition,    // Trzykrotne powt�rzenie pozycji
    FiftyMoveRule,          // Regu�a 50 posuni��
    InsufficientMaterial,   // Niewystarczaj�cy materia� do mata
    Agreement               // Remis za obop�ln� zgod�
}
