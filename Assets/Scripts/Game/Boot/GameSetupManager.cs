using System;
using Core.Boot;
using Core.Config;
using Core.Interfaces;
using Core.Settings;
using Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Boot
{
    public class GameSetupManager : MonoBehaviour
    {
        public static GameSetupManager Instance { get; private set; }

        [SerializeField] GameConfig gameConfig;

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

            switch (GameSettingsStore.CurrentSettings.GameMode)
            {
                case (GameMode.HumanVsHuman):
                    SceneManager.LoadScene("UI_Online", LoadSceneMode.Additive);
                    GameEvents.RequestNetworkPlayerPrefabSpawn();
                    break;
                case (GameMode.HumanVsBot):
                    Player1 = Instantiate(gameConfig.humanPlayerPrefab).GetComponent<IPlayerController>();
                    Player2 = Instantiate(gameConfig.botPlayerPrefab).GetComponent<IPlayerController>();
                    Player1.Initialize(GameSettingsStore.CurrentSettings.HostColor);
                    Player2.Initialize(GameSettingsStore.CurrentSettings.ClientColor);
                    SceneManager.LoadScene("UI_Offline", LoadSceneMode.Additive);
                    break;
                default:
                    throw new Exception("Unsupported game mode.");
            }
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
