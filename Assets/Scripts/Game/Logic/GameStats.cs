using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;
    public int whiteMoves {  get; private set; }
    public int blackMoves { get; private set; }
    public ChessColor currentTurnColor {  get; set; }
    public float whiteTime {  get; private set; }
    public float blackTime {  get; private set; }


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentTurnColor = ChessColor.White;
        whiteMoves = 0;
        blackMoves = 0;
        if(GameConfigStore.CurrentConfig.GameMode == GameMode.HumanVsHuman)
        {
            //do ewentualnej zmiany w ustawieniach gry w lobby
            whiteTime = 15f;
            blackTime = 15f;
        }
    }
    private void Start()
    {
        GameEvents.OnChangeTurnRequested += ChangeCurrentTurn;
        GameEvents.OnAddPlayerMoveRequested += AddPlayerMoves;
    }
    private void AddPlayerMoves()
    {
        if (currentTurnColor == ChessColor.White)
            AddWhiteMoves();
        else
            AddBlackMoves();
    }
    private void AddWhiteMoves()
    {
        whiteMoves++;
    }
    private void AddBlackMoves()
    {
        blackMoves++;
    }
    private void ChangeCurrentTurn()
    {
        if (currentTurnColor == ChessColor.White)
            currentTurnColor = ChessColor.Black;
        else
            currentTurnColor = ChessColor.White;
    }
}
