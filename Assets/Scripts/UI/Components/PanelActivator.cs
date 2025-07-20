using UnityEngine;

public class PanelActivator : MonoBehaviour
{
    public void ActivePanel()
    {
        gameObject.SetActive(true);
    }
    public void DisactivePanel()
    {
        gameObject.SetActive(false);
    }
}
