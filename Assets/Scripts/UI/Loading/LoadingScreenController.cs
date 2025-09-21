using Core.Boot;
using Core.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Loading
{
    public class LoadingScreenController : MonoBehaviour, ILoadingUI
    {
        public static LoadingScreenController Instance { get; private set; }

        [SerializeField] Slider progressBar;
        [SerializeField] TextMeshProUGUI progressText;

        void Awake()
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
}