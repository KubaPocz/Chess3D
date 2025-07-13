using UnityEngine;

public class GameSetupManager : MonoBehaviour
{
    public GameObject humanPrefab;
    public GameObject botPrefab;
    private void Start()
    {
        IPlayerController player1;
        IPlayerController player2;
        switch (GameConfigStore.CurrentConfig.GameMode)
        {
            case (GameMode.HumanVsHuman):
                player1 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                player2 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                break;
            case (GameMode.HumanVsBot):
                player1 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                player2 = Instantiate(botPrefab).GetComponent<IPlayerController>();
                break;
            default:
                throw new System.Exception("Unsupported game mode.");
        }
        ChessColor player1Color = GameConfigStore.CurrentConfig.PlayerColor;
        ChessColor player2Color = GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White ? ChessColor.Black : ChessColor.White;
        player1.Initialize(player1Color);
        player2.Initialize(player2Color);

        GameManager.Instance.AssignPlayers(player1, player2);
        GameManager.Instance.StartGame();
    }
}
