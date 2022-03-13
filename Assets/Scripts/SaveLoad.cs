using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
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

    private void Save()
    {
        var data = FindObjectOfType<DrawingCanvas>().GetTexture().EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/canvasSave.txt", data);
        
        Debug.Log("Saving canvas to: " + Application.dataPath + "/canvasSave.txt");
    }

    private void Load()
    {
        var data = File.ReadAllBytes(Application.dataPath + "/canvasSave.txt");
        FindObjectOfType<DrawingCanvas>().LoadTexture(data);
        
        Debug.Log("Loading canvas from: " + Application.dataPath + "/canvasSave.txt");
    }
}
