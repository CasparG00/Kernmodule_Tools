using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    private Color[] original;
    private Color[] backup;
    private DrawingCanvas canvas;

    private void Start()
    {
        canvas = FindObjectOfType<DrawingCanvas>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        var data = canvas.GetTexture().EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/canvasSave.txt", data);
        
        Debug.Log("Saving canvas to: " + Application.dataPath + "/canvasSave.txt");
    }

    public void Load()
    {
        backup = canvas.GetTexture().GetPixels();
        var path = Application.dataPath + "/canvasSave.txt";
        if (!File.Exists(path))
        {
            Debug.Log("<color=orange>Error</color>: No Save File Present!");
            return;
        }
        var data = File.ReadAllBytes(path);
        canvas.LoadTexture(data);
        original = canvas.GetTexture().GetPixels();
        SendSaveLoadCommand(original, backup, canvas.GetTexture());

        Debug.Log("Loaded canvas from: " + path);
    }
    
    private void SendSaveLoadCommand(Color[] original, Color[] backup, Texture2D texture)
    {
        var state = new SaveLoadCommand(original, backup, texture);
        CommandHandler.Add(state);
    }
}
