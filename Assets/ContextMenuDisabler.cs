using UnityEngine;

public class ContextMenuDisabler : MonoBehaviour
{
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    
    private void LateUpdate()
    {
        var mousePos = Input.mousePosition;
        var rectSize = rect.rect;
        if (mousePos.x < rect.position.x || mousePos.y > rect.position.y || mousePos.x > rect.position.x + rectSize.width || mousePos.y < rect.position.y - rectSize.height)
        {
            Debug.Log(rectSize.x);
            gameObject.SetActive(false);
        }
    }
}
