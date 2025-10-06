using UnityEngine;

namespace Core.Boot
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public GameObject humanPlayerPrefab;
        public GameObject botPlayerPrefab;
        public GameObject networkHumanPlayerPrefab;
        public GameObject whiteCamera;
        public GameObject blackCamera;
    }
}
