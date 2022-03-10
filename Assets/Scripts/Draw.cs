using System.Collections.Generic;
using UnityEngine;

public class Draw : ICommand
{
    private PixelData[] data;
    private List<PixelData> backupData  = new List<PixelData>();
    private Texture2D texture;

    public Draw(PixelData[] data, Texture2D texture)
    {
        this.data = data;

        foreach (var d in this.data)
        {
            var backup = new PixelData(d.position, texture.GetPixel(d.position.x, d.position.y));
            backupData.Add(backup);
        }
        
        this.texture = texture;
    }
    
    public void Execute()
    {
        foreach (var d in data)
        {
            texture.SetPixel(d.position.x, d.position.y, d.color);
        }
        texture.Apply();
    }

    public void Undo()
    {
        foreach (var d in backupData)
        {
            texture.SetPixel(d.position.x, d.position.y, d.color);
        }
        texture.Apply();
    }
}

public class PixelData
{
    public Vector2Int position;
    public Color color;

    public PixelData(Vector2Int position, Color color)
    {
        this.position = position;
        this.color = color;
    }
}
