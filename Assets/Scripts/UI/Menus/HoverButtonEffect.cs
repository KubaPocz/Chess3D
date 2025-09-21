using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menus
{
    public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] RectTransform leftImage;
        [SerializeField] RectTransform rightImage;

        FontWeight _orginalWeight;
        Color _orginalColor;
        void Start()
        {
            if (buttonText != null)
            {
                _orginalWeight = buttonText.fontWeight;
                _orginalColor = buttonText.color;
            }

            float textHeight = buttonText.fontSize;

            SetSquareSize(leftImage, textHeight);
            SetSquareSize(rightImage, textHeight);

            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (buttonText != null)
            {
                buttonText.fontWeight = FontWeight.Bold;
                buttonText.color = Color.white;
            }

            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonText != null)
            {
                buttonText.fontWeight = _orginalWeight;
                buttonText.color = _orginalColor;
            }

            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
        }
        void SetSquareSize(RectTransform img, float height)
        {
            img.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            img.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height); // kwadrat
        }
    }
}
