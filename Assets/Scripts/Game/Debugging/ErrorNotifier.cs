using UnityEngine;

public class ErrorNotifier : MonoBehaviour
{
    static ErrorNotifier Instance;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        GameEvents.OnNotifyError += LogError;
    }
    private void OnDisable()
    {
        GameEvents.OnNotifyError -= LogError;
    }
    private void LogError(string error)
    {
        Debug.Log(error);
    }
}
