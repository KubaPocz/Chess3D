using Codice.Client.Common.GameUI;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController Instance { get; private set; }
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
    [SerializeField] TMP_InputField LobbyCodeInput;
    [SerializeField] Button ConfirmJoiningLobbyButton;
    [SerializeField] Button BackToPlayPanelButtonOnline;

    [Header("OnlineLobbyPanel")]
    [SerializeField] Animator OnlineLobbyPanelAnimator;
    [SerializeField] TextMeshProUGUI HostName;
    [SerializeField] TextMeshProUGUI ClientName;
    [SerializeField] Button LeaveLobbyButton;

    [Header("LobbyHostElements")]
    [SerializeField] TextMeshProUGUI LobbyCode;
    [SerializeField] Button KickClientButton;
    [SerializeField] Button StartGameButton;

    private ChessColor playerColor;
    private int gameDifficulty;

    private Dictionary<string, string> lobbyInfo = new Dictionary<string, string>();
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        PlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
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
        HostOnlineGameButton.onClick.AddListener(() => {GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, OnlineLobbyPanelAnimator); GameEvents.RequestCreateLobby(); });
        JoinLobbyButton.onClick.AddListener(() => ShowJoinSection());
        ConfirmJoiningLobbyButton.onClick.AddListener(() =>
        {
            string code = LobbyCodeInput.text.Trim().ToUpper();
            if (!string.IsNullOrEmpty(code))
            {
                Debug.Log(code);
                GameEvents.RequestHidePanel(OnlineLobbyPanelAnimator, OnlineLobbyPanelAnimator);
                GameEvents.RequestJoinLobby(code);
            }
        }); 
        BackToPlayPanelButtonOnline.onClick.AddListener(() => { GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, PlayPanelAnimator); GameEvents.RequestLeaveLobby(); });
        
        //LobbyPanel
        LeaveLobbyButton.onClick.AddListener(() => GameEvents.RequestHidePanel(OnlineLobbyPanelAnimator,PlayPanelAnimator));
        KickClientButton.onClick.AddListener(() => GameEvents.RequestKickClient());
        StartGameButton.onClick.AddListener(() => GameEvents.RequestStartGameOnline());

        PlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OfflinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OnlinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OnlineLobbyPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        JoinLobbySection.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnHidePanelRequested += HidePanel;
        GameEvents.OnColorChangeRequested += SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested += SetGameDifficulty;
        GameEvents.OnLobbyCodeUpdateRequested += UpdateLobbyCodeLabel;
    }
    private void OnDisable()
    {
        GameEvents.OnHidePanelRequested -= HidePanel;
        GameEvents.OnColorChangeRequested -= SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested -= SetGameDifficulty;
        GameEvents.OnLobbyCodeUpdateRequested -= UpdateLobbyCodeLabel;
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
    private void UpdateLobbyCodeLabel(string code)
    {
        LobbyCode.text = code;
        lobbyInfo.Add("code", code);
    }
}
