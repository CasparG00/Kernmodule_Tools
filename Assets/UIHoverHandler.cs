using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnEnable()
    {
        var children = transform.childCount;
        for (var i = 1; i < children; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var children = transform.childCount;
        for (var i = 1; i < children; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var children = transform.childCount;
        for (var i = 1; i < children; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
