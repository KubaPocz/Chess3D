using UnityEngine;

public class BotPlayerController : MonoBehaviour, IPlayerController
{
    public void StartTurn()
    {
        enabled = true;
    }
    public void EndTurn()
    {
        enabled = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
