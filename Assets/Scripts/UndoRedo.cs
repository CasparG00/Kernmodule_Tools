using UnityEngine;

public class UndoRedo : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Undo();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Redo();
        }
    }
    
    public void Undo() 
    {
        CommandHandler.Undo();
    }

    public void Redo()
    {
        CommandHandler.Redo();
    }
}
