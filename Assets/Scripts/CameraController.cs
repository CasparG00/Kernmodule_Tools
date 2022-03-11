using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float sensitivity;
    [SerializeField] private float zoomSpeed;
    [SerializeField] [Min(0)] private float maxZoom = 0.1f;

    private void Update()
    {
        if (Input.GetMouseButton(2))
        {
            var mouseInput = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            cam.transform.position += mouseInput * sensitivity * cam.orthographicSize;
        }

        var mouse = Input.mousePosition;
        if (mouse.x < 0 || mouse.y < 0 || mouse.x > Screen.width || mouse.y > Screen.height) return;
        cam.orthographicSize += -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        cam.orthographicSize = Mathf.Max(cam.orthographicSize, maxZoom);
    }
}
