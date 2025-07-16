using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HoverButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] RectTransform leftImage;
    [SerializeField] RectTransform rightImage;

    private FontWeight orginalWeight;
    private Color orginalColor;
    void Start()
    {
        if (buttonText != null)
        {
            orginalWeight = buttonText.fontWeight;
            orginalColor = buttonText.color;
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
            buttonText.fontWeight = orginalWeight;
            buttonText.color = orginalColor;
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
