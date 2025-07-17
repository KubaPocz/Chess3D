using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("MainPanel")]
    [SerializeField] Animator MainPanelAnimator;
    [SerializeField] Button PlayButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button ExitButton;

    [Header("PlayPanel")]
    [SerializeField] Animator PlayPanelAnimator;
    [SerializeField] Button OnlinePlayButton;
    [SerializeField] Button OfflinePlayButton;
    [SerializeField] Button BackToMenuButton;

    [Header("OfflinePlayPanel")]
    [SerializeField] Animator OfflinePlayPanelAnimator;
    [SerializeField] Button StartGameOfflineButton;
    [SerializeField] Button BackToPlayPanelButton;

    private ChessColor playerColor;
    private int gameDifficulty;
    private void Start()
    {
        PlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator,PlayPanelAnimator));
        OptionsButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
        ExitButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));

        //onlineplaybutton
        OfflinePlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, OfflinePlayPanelAnimator));
        BackToMenuButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, MainPanelAnimator));

        StartGameOfflineButton.onClick.AddListener(() => GameEvents.RequestStartGameOffline(playerColor, gameDifficulty));
        BackToPlayPanelButton.onClick.AddListener(() => GameEvents.RequestHidePanel(OfflinePlayPanelAnimator, PlayPanelAnimator));

        PlayPanelAnimator.gameObject.SetActive(false);
        OfflinePlayPanelAnimator.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnHidePanelRequested += HidePanel;
        GameEvents.OnColorChangeRequested += SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested += SetGameDifficulty;
    }
    private void OnDisable()
    {
        GameEvents.OnHidePanelRequested -= HidePanel;
        GameEvents.OnColorChangeRequested -= SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested -= SetGameDifficulty;
    }
    private void HidePanel(Animator panelHide, Animator panelShow)
    {
        panelHide.SetTrigger("HidePanel");

        panelShow.gameObject.GetComponent<PanelActivator>().ActivePanel();
        panelShow.SetTrigger("ShowPanel");
    }
    private void SetPlayerColor(ChessColor color)
    {
        playerColor = color;
    }
    private void SetGameDifficulty(int difficulty)
    {
        gameDifficulty = difficulty;
    }
}
