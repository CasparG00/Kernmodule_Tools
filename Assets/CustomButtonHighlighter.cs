using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    private Color normal;
    private Color highlight;
    
    private void OnEnable()
    {
        image = GetComponent<Image>();
        var button = GetComponent<Button>();

        normal = button.colors.normalColor;
        highlight = button.colors.highlightedColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = highlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normal;
    }
}
