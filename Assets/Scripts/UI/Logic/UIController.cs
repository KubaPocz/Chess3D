using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("MainPanel")]
    [SerializeField] Animator MainPanelAnimator;
    [SerializeField] Button PlayButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button ExitButton;

    [Header("PlayePanel")]
    [SerializeField] Animator PlayPanelAnimator;


    private void Start()
    {
        PlayButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator,PlayPanelAnimator));
        OptionsButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));
        ExitButton.onClick.AddListener(() => GameEvents.RequestHidePanel(MainPanelAnimator, PlayPanelAnimator));

        PlayPanelAnimator.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.OnHidePanelRequested += HidePanel;
    }
    private void OnDisable()
    {
        GameEvents.OnHidePanelRequested -= HidePanel;
    }
    private void HidePanel(Animator panelHide, Animator panelShow)
    {
        panelHide.SetTrigger("HidePanel");

        panelShow.gameObject.GetComponent<PanelActivator>().ActivePanel();
        panelShow.SetTrigger("ShowPanel");
    }
}
