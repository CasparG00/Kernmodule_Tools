using System.Linq;
using UnityEditor;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    [SerializeField] private Vector2Int canvasScale;
    private Texture2D texture;
    
    private void Start()
    {
        var rawImage = GetComponent<SpriteRenderer>();
        rawImage.sprite = GenerateSprite("newDrawing", canvasScale.x, canvasScale.y);
    }

    private Sprite GenerateSprite(string name, int width, int height)
    {
        texture = new Texture2D(width, height)
        {
            name = name,
            filterMode = FilterMode.Point,
        };

        var pixels = Enumerable.Repeat(Color.white, texture.width * texture.height).ToArray();
        texture.SetPixels(pixels);
        texture.Apply();

        var spriteRect = new Rect(0, 0, texture.width, texture.height);
        var sprite = Sprite.Create(texture, spriteRect, Vector2.one * 0.5f, width);
        sprite.name = name + "Sprite";

        return sprite;
    }

    public void LoadTexture(byte[] data)
    {
        texture.LoadImage(data);
    }

    public Texture2D GetTexture()
    {
        return texture;
    }
}