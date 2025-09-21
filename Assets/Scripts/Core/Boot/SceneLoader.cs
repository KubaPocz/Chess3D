using System.Collections;
using Core.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Boot
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;
        public static string SceneToLoad;
        static ILoadingUI _loadingUI;
        void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        void Start()
        {
            StartCoroutine(LoadSceneAsync(SceneToLoad));
        }
        public static void SetLoadingUI(ILoadingUI ui)
        {
            _loadingUI = ui;
        }
        public static void UpdateProgress(float progress)
        {
            _loadingUI?.UpdateProgress(progress);
        }
        IEnumerator LoadSceneAsync(string SceneToLoad)
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
}
