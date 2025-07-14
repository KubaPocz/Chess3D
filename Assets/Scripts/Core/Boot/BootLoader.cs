using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BootLoader : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GameConfigStore.CurrentConfig = new GameConfig(GameMode.HumanVsBot,ChessColor.White);
        SceneLoader.SceneToLoad = "Gameboard";
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }
}
