using System;
using Core.Config;
using Core.Interfaces;
using Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Boot
{
    public class GameSetupManager : MonoBehaviour
    {
        public static GameSetupManager Instance { get; private set; }

        [SerializeField] public GameObject humanPrefab;
        [SerializeField] public GameObject botPrefab;
        [SerializeField] public Camera whiteCamera;
        [SerializeField] public Camera blackCamera;

        public IPlayerController Player1 { get;  set; }
        public IPlayerController Player2 { get;  set; }
        public static event Action<IPlayerController, IPlayerController> OnPlayersReady;
        void Awake()
        {
            Debug.Log($"[GameSetupManager.Awake] executing at t={Time.time}");

            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            switch (GameConfigStore.CurrentConfig.GameMode)
            {
                case (GameMode.HumanVsHuman):
                    SceneManager.LoadScene("UI_Online", LoadSceneMode.Additive);
                    break;
                case (GameMode.HumanVsBot):
                    Player1 = Instantiate(humanPrefab).GetComponent<IPlayerController>();
                    Player2 = Instantiate(botPrefab).GetComponent<IPlayerController>();
                    SceneManager.LoadScene("UI_Offline", LoadSceneMode.Additive);
                    break;
                default:
                    throw new Exception("Unsupported game mode.");
            }
            whiteCamera.gameObject.SetActive(GameConfigStore.CurrentConfig.PlayerColor == ChessColor.White);
            blackCamera.gameObject.SetActive(GameConfigStore.CurrentConfig.PlayerColor == ChessColor.Black);
        }

        public void RegisterPlayer(IPlayerController controller)
        {
            if (Player1 == null) Player1 = controller;
            else if (Player2 == null) Player2 = controller;

            if (Player1 != null && Player2 != null)
            {
                OnPlayersReady?.Invoke(Player1, Player2);
            }
        }
        void OnDestroy()
        {
            Instance = null;
        }
    }
}
