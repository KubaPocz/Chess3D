using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class PlayFromBoot
{
    static string bootScenePath = "Assets/Scenes/Boot.unity";
    static string previousScenePath;

    static PlayFromBoot()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            previousScenePath = SceneManager.GetActiveScene().path;

            if (!EditorSceneManager.GetSceneByPath(bootScenePath).isLoaded)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(bootScenePath);
            }
        }

        if (state == PlayModeStateChange.EnteredEditMode && !string.IsNullOrEmpty(previousScenePath))
        {
            EditorSceneManager.OpenScene(previousScenePath);
        }
    }
}