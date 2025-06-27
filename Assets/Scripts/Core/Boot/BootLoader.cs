using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "MainMenu";
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    private void Start()
    {
        StartCoroutine(LoadMainMenuAsync());
        loadingBar.value = 0;
    }

    private IEnumerator LoadMainMenuAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingBar.value = progress;
            loadingText.text=$"{progress*100f}%";
            if (progress >= 1f) 
                asyncLoad.allowSceneActivation = true;
            yield return null;
        }
        
    }
}
