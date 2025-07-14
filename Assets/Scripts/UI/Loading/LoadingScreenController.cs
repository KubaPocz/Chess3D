using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenController : MonoBehaviour, ILoadingUI
{
    public static LoadingScreenController Instance { get; private set; }

    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;

    private void Awake()
    {
        Instance = this;
        SceneLoader.SetLoadingUI(this);
    }
    public void UpdateProgress(float progress)
    {
        progressBar.value = progress;
        progressText.text = $"{progress * 100f:0}%";
    }
}