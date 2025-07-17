using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ChessColor color;
    public Image targetGraphic;
    public Color onColor = Color.white;
    public Color offColor = new Color(1, 1, 1, 0.1f);
    public Color hoverColor = new Color(1, 1, 1, 0.3f);

    private Toggle toggle;
    private bool isHovered = false;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        UpdateVisual();
    }
    private void OnToggleValueChanged(bool isOn)
    {
        UpdateVisual();
        if (isOn)
        {
            GameEvents.RequestColorChange(color);
        }
        Debug.Log($"{color} toggle changed: isOn = {isOn}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        UpdateVisual();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (toggle.isOn)
            targetGraphic.color = onColor;
        else if (isHovered)
            targetGraphic.color = hoverColor;
        else
            targetGraphic.color = offColor;
    }
}
