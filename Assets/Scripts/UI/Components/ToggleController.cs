using Core.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public ChessColor color;
        public Image targetGraphic;
        public Color onColor = Color.white;
        public Color offColor = new(1, 1, 1, 0.1f);
        public Color hoverColor = new(1, 1, 1, 0.3f);

        Toggle _toggle;
        bool _isHovered = false;

        void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);

            UpdateVisual();
        }
        void OnToggleValueChanged(bool isOn)
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
            _isHovered = true;
            UpdateVisual();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (_toggle.isOn)
                targetGraphic.color = onColor;
            else if (_isHovered)
                targetGraphic.color = hoverColor;
            else
                targetGraphic.color = offColor;
        }
    }
}
