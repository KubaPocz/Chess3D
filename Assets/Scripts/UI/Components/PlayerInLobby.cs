using Core.Utilities;
using TMPro;
using UnityEngine;

namespace UI.Components
{
    public class PlayerInLobby : MonoBehaviour
    {
        public string playerName = "";
        public ChessColor playerColor = ChessColor.White;
        public void UpdatePlayerInfo(string playerName)
        {
            playerName = playerName.Trim();
            gameObject.GetComponent<TextMeshProUGUI>().text = playerName;
        }
    }
}
