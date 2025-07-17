using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public static string SceneToLoad;
    private static ILoadingUI loadingUI;
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
        StartCoroutine(LoadSceneAsync(SceneToLoad));
    }
    public static void SetLoadingUI(ILoadingUI ui)
    {
        loadingUI = ui;
    }
    public static void UpdateProgress(float progress)
    {
        loadingUI?.UpdateProgress(progress);
    }
    private IEnumerator LoadSceneAsync(string SceneToLoad)
    {
        yield return null;

        AsyncOperation asyncLoading = SceneManager.LoadSceneAsync(SceneToLoad);
        asyncLoading.allowSceneActivation = false;
        
        while (asyncLoading.progress<0.9f)
        {
            float progress = Mathf.Clamp01(asyncLoading.progress / 0.9f);
            UpdateProgress(progress);
            yield return null;
        }
        UpdateProgress(1f);
        yield return new WaitForSeconds(0.5f);
        asyncLoading.allowSceneActivation = true;
    }
}
