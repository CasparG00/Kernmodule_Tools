using UnityEngine;

public class Draw : ICommand
{
    private Color[] original;
    private Color[] backup;
    private Texture2D texture;

    public Draw(Color[] original, Color[] backup, Texture2D texture)
    {
        this.original = original;
        this.backup = backup;
        this.texture = texture;
    }
    
    public void Execute()
    {
        texture.SetPixels(original);
        texture.Apply();
    }

    public void Undo()
    {
        texture.SetPixels(backup);
        texture.Apply();
    }
}