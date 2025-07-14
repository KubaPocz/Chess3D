using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class PlayFromBoot
{
    static PlayFromBoot()
    {
        var boot = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Boot.unity");
        if (boot != null)
            EditorSceneManager.playModeStartScene = boot;
    }
}
