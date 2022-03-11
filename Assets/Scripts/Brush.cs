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

    private Texture2D texture;
    private RaycastHit2D hit;

    /// TODO: Move Mouse Commands outside of raycast statement.

    private void Update()
    {
        if (texture != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                backup = texture.GetPixels();
            }
        }

        if (!hit)
        {
            lastTexCoord = texCoord;
        }
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit)
        {
            var sr = hit.transform.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                texture = sr.sprite.texture;

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

                if (Input.GetMouseButtonUp(0))
                {
                    original = texture.GetPixels();
                    SendDrawCommand(original, backup, texture);
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

    private void PlotLine(Vector2Int start, Vector2Int end, Texture2D tex, Color color)
    {
        var dx = Mathf.Abs(end.x - start.x);
        var sx = start.x < end.x ? 1 : -1;
        var dy = -Mathf.Abs(end.y - start.y);
        var sy = start.y < end.y ? 1 : -1;
        var error = dx + dy;

        while (true)
        {
            DrawCircle(tex, color, start, brushSize);
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
    
    private void DrawCircle(Texture2D tex, Color color, Vector2Int pos, int radius)
    {
        float rSquared = radius * radius;

        for (var u = pos.x - radius; u < pos.x + radius + 1; u++)
        for (var v = pos.y - radius; v < pos.y + radius + 1; v++)
        {
            if (!((pos.x - u) * (pos.x - u) + (pos.y - v) * (pos.y - v) < rSquared)) continue;
            if (u < 0 || v < 0 || u >= tex.width || v >= tex.height) continue;
            tex.SetPixel(u, v, color);
        }
    }
}
