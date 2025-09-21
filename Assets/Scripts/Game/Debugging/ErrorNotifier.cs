using UnityEngine;

namespace Game.Debugging
{
    public class ErrorNotifier : MonoBehaviour
    {
        static ErrorNotifier _instance;
        void Awake()
        {
            if(_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void OnEnable()
        {
            GameEvents.OnNotifyError += LogError;
        }
        void OnDisable()
        {
            GameEvents.OnNotifyError -= LogError;
        }
        void LogError(string error)
        {
            Debug.Log(error);
        }
    }
}
