using System.IO;
using UnityEngine;

public class MapGenerationManager : MonoBehaviour
{
    private Texture2D texture;
    private SpriteRenderer spriteRenderer;
    private DrawingCanvas canvas;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        canvas = FindObjectOfType<DrawingCanvas>();
    }
    
    public void GenerateMap()
    {
        var tex = canvas.GetTexture();

        texture = new Texture2D(tex.width, tex.height)
        {
            name = "map",
            filterMode = FilterMode.Point
        };
        texture.SetPixels(tex.GetPixels());
        texture.Apply();
        
        texture = MapGenerator.Generate(texture, texture.width * 8, texture.height * 8);
        texture.Apply();

        spriteRenderer.sprite = GenerateSprite(texture);
    }
    
    private Sprite GenerateSprite(Texture2D original)
    {
        var spriteRect = new Rect(0, 0, original.width, original.height);
        var sprite = Sprite.Create(original, spriteRect, Vector2.one * 0.5f, original.width);
        
        sprite.name = original.name + "Sprite";

        return sprite;
    }
    
    public void ExportImage()
    {
        SaveImageToFile();
    }

    private void SaveImageToFile()
    {
        var bytes = texture.EncodeToPNG();

        var path = Application.dataPath + "/Maps";
        
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        path = path + "/map" + ".png";

        File.WriteAllBytes(path, bytes);
        
        // Open File in saved location
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
}
