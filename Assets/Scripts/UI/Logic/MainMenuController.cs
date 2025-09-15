using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MainMenuController : MonoBehaviour
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

    [Header("ProfileCreationPanel")]
    [SerializeField] Animator ProfileCreationPanelAnimator;
    [SerializeField] Button BackToPlayPanelProfileCreation;
    [SerializeField] TMP_InputField PlayerName;
    [SerializeField] Button CreateProfileButton;

    [Header("OnlinePlayPanel")]
    [SerializeField] Animator OnlinePlayPanelAnimator;
    [SerializeField] Button HostOnlineGameButton;
    [SerializeField] Button JoinLobbyButton;
    [SerializeField] Button BackToPlayPanelButtonOnline;

    [Header("JoinLobbyPanel")]
    [SerializeField] Animator JoinLobbyPanelAnimator;
    [SerializeField] TMP_InputField LobbyCodeInput;
    [SerializeField] Button ConfirmCodeButton;
    [SerializeField] Button BackToOnlinePanelJoin;

    [Header("LobbyPanel")]
    [SerializeField] Animator LobbyPanelAnimator;
    [SerializeField] GameObject PlayersInLobbyContainer;
    [SerializeField] Button SwapTeamsButtton;
    [SerializeField] Button StartGameOnlineButton;
    [SerializeField] Button BackToOnlinePanelLobby;
    [SerializeField] TextMeshProUGUI codeLabel;
    [SerializeField] GameObject PlayerInLobby_PREFAB;

    private ChessColor playerColor;
    private int gameDifficulty;
    private readonly List<string> currentPlayers = new List<string>();
    private void Start()
    {
        PlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
        //do zmiany panele
        OptionsButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
        ExitButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));


        //Play Panel
        OnlinePlayButton.onClick.AddListener(GoOnlinePlay);
        OfflinePlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, OfflinePlayPanelAnimator));
        BackToMenuButton.onClick.AddListener(() => GameEvents.RequestHidePanel(PlayPanelAnimator, MainPanelAnimator));

        //Profile Creation Panel
        CreateProfileButton.onClick.AddListener(() => CreateProfile(PlayerName.text));
        BackToPlayPanelProfileCreation.onClick.AddListener(() => GameEvents.RequestHidePanel(ProfileCreationPanelAnimator, PlayPanelAnimator));

        //Offline Panel
        StartGameOfflineButton.onClick.AddListener(() => GameEvents.RequestStartGameOffline(playerColor, gameDifficulty));
        BackToPlayPanelButtonOffline.onClick.AddListener(() => GameEvents.RequestHidePanel(OfflinePlayPanelAnimator, PlayPanelAnimator));

        //Online Panel
        HostOnlineGameButton.onClick.AddListener(() => { GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, LobbyPanelAnimator); GameEvents.RequestCreateLobby(); SetSwapTeamsVisibility(true); });
        JoinLobbyButton.onClick.AddListener(() => { GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, JoinLobbyPanelAnimator);SetSwapTeamsVisibility(false); });
        BackToPlayPanelButtonOnline.onClick.AddListener(() => GameEvents.RequestHidePanel(OnlinePlayPanelAnimator, PlayPanelAnimator));

        //JoinLobby
        ConfirmCodeButton.onClick.AddListener(() => JoinLobbyByCode(LobbyCodeInput.text));
        BackToOnlinePanelJoin.onClick.AddListener(() => GameEvents.RequestHidePanel(JoinLobbyPanelAnimator, OnlinePlayPanelAnimator));

        //LobbyPanel
        BackToOnlinePanelLobby.onClick.AddListener(() => { GameEvents.RequestHidePanel(LobbyPanelAnimator, OnlinePlayPanelAnimator); LeaveLobby(); });
        SwapTeamsButtton.onClick.AddListener(() => GameEvents.RequestSwapTeams());
        StartGameOnlineButton.onClick.AddListener(() => GameEvents.RequestStartGameOnline(GameConfigStore.CurrentConfig.PlayerColor));

        PlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        ProfileCreationPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OfflinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        OnlinePlayPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        JoinLobbyPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
        LobbyPanelAnimator.gameObject.GetComponent<PanelActivator>().DisactivePanel();
    }
    private void OnEnable()
    {
        GameEvents.OnHidePanelRequested += HidePanel;
        GameEvents.OnColorChangeRequested += SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested += SetGameDifficulty;
        GameEvents.LobbyCreated += OnLobbyCreated;
        GameEvents.LobbyJoined += OnLobbyJoined;
        GameEvents.OnPlayersListUpdated += UpdatePlayersInLobbyUI;
        GameEvents.LobbyClosedByHost += OnLobbyClosedByHost;
        GameEvents.LobbyLeftOrDeleted += OnLobbyLeftOrDeleted;
    }
    private void OnDisable()
    {
        GameEvents.OnHidePanelRequested -= HidePanel;
        GameEvents.OnColorChangeRequested -= SetPlayerColor;
        GameEvents.OnGameDifficultyChangeRequested -= SetGameDifficulty;
        GameEvents.LobbyCreated -= OnLobbyCreated;
        GameEvents.LobbyJoined -= OnLobbyJoined;
        GameEvents.OnPlayersListUpdated -= UpdatePlayersInLobbyUI;
        GameEvents.LobbyClosedByHost -= OnLobbyClosedByHost;
        GameEvents.LobbyLeftOrDeleted -= OnLobbyLeftOrDeleted;
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
    private void GoOnlinePlay()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            GameEvents.RequestHidePanel(PlayPanelAnimator, OnlinePlayPanelAnimator);
        else
            GameEvents.RequestHidePanel(PlayPanelAnimator, ProfileCreationPanelAnimator);
    }
    private void CreateProfile(string playerName)
    {
        if (playerName.Length >= 7 && playerName != null && !int.TryParse(playerName, out int id))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            GameEvents.RequestHidePanel(ProfileCreationPanelAnimator, OnlinePlayPanelAnimator);
        }
    }
    private void JoinLobbyByCode(string code)
    {
        GameEvents.RequestJoinByCode(code);
    }
    private void LeaveLobby()
    {
        Debug.Log("Left");
        GameEvents.RequestLeaveOrDelete();
    }

    void OnLobbyCreated(string id, string code)
    {
        codeLabel.text = code;
        // mo¿esz te¿ automatycznie skopiowaæ do schowka
        // GUIUtility.systemCopyBuffer = code;
    }
    void OnLobbyJoined(string id, string code)
    {
        codeLabel.text = code;
        HidePanel(JoinLobbyPanelAnimator, LobbyPanelAnimator);
    }
    void SetSwapTeamsVisibility(bool isHost)
    {
        SwapTeamsButtton.gameObject.SetActive(isHost);
        StartGameOnlineButton.gameObject.SetActive(isHost);
    }
    void UpdatePlayersInLobbyUI(List<string> players)
    {
        // Wyczyœæ star¹ listê
        foreach (Transform child in PlayersInLobbyContainer.transform)
            Destroy(child.gameObject);

        currentPlayers.Clear();

        // Dodaj aktualnych graczy
        foreach (var name in players)
        {
            currentPlayers.Add(name);
            var entry = Instantiate(PlayerInLobby_PREFAB, PlayersInLobbyContainer.transform);
            entry.GetComponentInChildren<TextMeshProUGUI>().text = name;
        }
    }
    void OnLobbyClosedByHost()
    {
        Debug.Log("Host zamkn¹³ lobby!");
        ClearPlayersUI();
        HidePanel(LobbyPanelAnimator, OnlinePlayPanelAnimator);
    }

    void OnLobbyLeftOrDeleted()
    {
        Debug.Log("Opuszczono lobby (klient sam wyszed³).");
        ClearPlayersUI();
    }
    void ClearPlayersUI()
    {
        foreach (Transform child in PlayersInLobbyContainer.transform)
            Destroy(child.gameObject);
        currentPlayers.Clear();
    }
}
