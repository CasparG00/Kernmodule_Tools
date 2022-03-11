using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] private Color drawColor = Color.black;
    [SerializeField] private Color eraseColor = Color.white;
    [SerializeField] private int brushSize = 5;

    private Vector2Int texCoord;
    private Vector2Int lastTexCoord;

    private Color[] original;
    private Color[] backup;

    private void Update()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            var sr = hit.transform.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                var texture = sr.sprite.texture;
                
                if (Input.GetMouseButtonDown(0))
                {
                    backup = texture.GetPixels();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    original = texture.GetPixels();
                    SendDrawCommand(original, backup, texture);
                }

                var coord = (Vector2) hit.transform.position - hit.point;
                coord *= -1;
                coord += Vector2.one * 0.5f;

                coord.x *= texture.width;
                coord.y *= texture.height;

                texCoord.x = (int) coord.x;
                texCoord.y = (int) coord.y;

                if (Input.GetMouseButton(0))
                {
                    PlotLine(lastTexCoord, texCoord, texture, drawColor);
                    texture.Apply();
                }

                if (Input.GetMouseButton(1))
                {
                    PlotLine(lastTexCoord, texCoord, texture, eraseColor);
                    texture.Apply();
                }
            }
        }
    }
    
    private void LateUpdate()
    {
        lastTexCoord = texCoord;
    }
    
    private void SendDrawCommand(Color[] original, Color[] backup, Texture2D texture)
    {
        var draw = new Draw(original, backup, texture);
        CommandHandler.instance.Add(draw);
    }

    private void PlotLine(Vector2Int start, Vector2Int end, Texture2D target, Color color)
    {
        var dx = Mathf.Abs(end.x - start.x);
        var sx = start.x < end.x ? 1 : -1;
        var dy = -Mathf.Abs(end.y - start.y);
        var sy = start.y < end.y ? 1 : -1;
        var error = dx + dy;

        while (true)
        {
            target.SetPixel(start.x, start.y, color);
            if (start.x == end.x && start.y == end.y) break;
            var e2 = 2 * error;
            if (e2 >= dy)
            {
                if (start.x == end.x) break;
                error += dy;
                start.x += sx;
            }

            if (e2 > dx) continue;
            if (start.y == end.y) break;
            error += dx;
            start.y += sy;
        }
    }
}
