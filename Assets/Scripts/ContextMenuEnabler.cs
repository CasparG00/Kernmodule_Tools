using UnityEngine;

public class ContextMenuEnabler : MonoBehaviour
{
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private Vector2Int offsetInPixels;

    private void OnEnable()
    {
        contextMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            contextMenu.SetActive(!contextMenu.activeSelf);
            contextMenu.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(offsetInPixels.x, offsetInPixels.y, 0);
        }
    }
}
