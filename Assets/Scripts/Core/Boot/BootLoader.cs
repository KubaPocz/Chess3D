using System.Collections;
using Core.Boot;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneLoader.SceneToLoad = "MainMenu";
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }
}
