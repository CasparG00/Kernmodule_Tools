using UnityEngine;
using UnityEngine.UI;

public class CustomHoverHandler : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect rect;
    private Image image;

    private Color normalColor;
    private Color highlightColor;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rect = rectTransform.rect;

        var button = GetComponent<Button>();
        normalColor = button.colors.normalColor;
        highlightColor = button.colors.highlightedColor;

        image = GetComponent<Image>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;
        if (mousePos.x < rect.position.x || mousePos.y > rect.position.y || mousePos.x > rect.position.x + rect.width || mousePos.y < rect.position.y - rect.height)
        {
            image.color = normalColor;
        }
        else
        {
            image.color = highlightColor;
        }
    }
}
