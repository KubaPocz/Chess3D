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
    [SerializeField] Button BackToPlayPanelButtonOffline;

    [Header("OnlinePlayPanel")]
    [SerializeField] Animator OnlinePlayPanelAnimator;
    [SerializeField] Button HostOnlineGameButton;
    [SerializeField] Button JoinLobbyButton;
    [SerializeField] GameObject JoinLobbySection;
    [SerializeField] Button ConfirmJoiningLobbyButton;
    [SerializeField] Button BackToPlayPanelButtonOnline;

    [Header("OnlineLobbyPanel")]
    [SerializeField] Animator OnlineLobbyPanelAnimator;

    private ChessColor playerColor;
    private int gameDifficulty;
    private void Start()
    {
        PlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator,PlayPanelAnimator));
        //do zmiany panele
        OptionsButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
        ExitButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));


        //Play Panel
        OnlinePlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, OnlinePlayPanelAnimator));
        OfflinePlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, OfflinePlayPanelAnimator));
        BackToMenuButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, MainPanelAnimator));

        //Offline Panel
        StartGameOfflineButton.onClick.AddListener(() => GameEvents.RequestStartGameOffline(playerColor, gameDifficulty));
        BackToPlayPanelButtonOffline.onClick.AddListener(() => GameEvents.RequestHidePanel(OfflinePlayPanelAnimator, PlayPanelAnimator));

        //OnlinePanel
        //HostOnlineGameButton.onClick.AddListener(() => GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, OnlineLobbyPanelAnimator));
        JoinLobbyButton.onClick.AddListener(() => ShowJoinSection());
        //ConfirmJoiningLobbyButton.onClick.AddListener(() => GameEvents.RequestHidePanel(OnlineLobbyPanelAnimator, OnlineLobbyPanelAnimator));
        BackToPlayPanelButtonOnline.onClick.AddListener(() => GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, PlayPanelAnimator));

        PlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OfflinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OnlinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        JoinLobbySection.SetActive(false);
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
    private void ShowJoinSection()
    {
        JoinLobbySection.SetActive(true);
    }
}
