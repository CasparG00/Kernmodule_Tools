using UnityEngine;

public class Fill : MonoBehaviour
{
    private void FloodFill(Vector2Int pos, Color targetColor, Color color, Texture2D tex)
    {

        if (tex.GetPixel(pos.x, pos.y) == color) return;
        if (tex.GetPixel(pos.x, pos.y) != targetColor) return;
   
        tex.SetPixel(pos.x, pos.y, color);

        var newPos = pos;
        newPos.x += 1;
        FloodFill(newPos, targetColor, color, tex);
        newPos = pos;
        newPos.x -= 1;
        FloodFill(newPos, targetColor, color, tex);
        newPos = pos;
        newPos.y += 1;
        FloodFill(newPos, targetColor, color, tex);
        newPos = pos;
        newPos.y -= 1;
        FloodFill(newPos, targetColor, color, tex);
    }
}
