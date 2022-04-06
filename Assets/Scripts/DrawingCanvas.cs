using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

    public void ImportImage()
    {
        var ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        //ofn.filter = "All Files\0*.*\0\0";
        ofn.filter = "Image Files\0*.png;*.jpg;*.jpeg;*.gif";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir =UnityEngine.Application.dataPath;//默认路径
        ofn.title = "Open Project";
        ofn.defExt = "JPG";//显示文件的类型
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags=0x00080000|0x00001000|0x00000800|0x00000200|0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        if(DllTest.GetOpenFileName(ofn))
        {
            LoadImageFromFile(ofn.file);//加载图片到panle
            Debug.Log( "Selected file with full path: {0}"+ofn.file );
        }
    }

    private void LoadImageFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var data = File.ReadAllBytes(filePath);
            texture.LoadImage(data);
        }
        else
        {
            Debug.Log($"Error: file at {filePath} does not exist!");
        }
    }
}