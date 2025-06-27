using UnityEngine;

public class HumanPlayerController : MonoBehaviour, IPlayerController
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
