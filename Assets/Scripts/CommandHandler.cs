using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : Singleton<CommandHandler>
{
    private readonly List<ICommand> commands = new();
    private int index;

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
    
    public void Add(ICommand command)
    {
        if (index < commands.Count)
        {
            commands.RemoveRange(index, commands.Count - index);
        }

        commands.Add(command);
        command.Execute();
        index++;
        
        Debug.Log($"Added: {command}");
    }

    public void Undo()
    {
        if (commands.Count == 0) return;
        if (index <= 0) return;
        commands[index - 1].Undo();
        Debug.Log($"Undo: {commands[index - 1]}");
        index--;
    }
    
    public void Redo()
    {
        if (commands.Count == 0) return;
        if (index >= commands.Count) return;
        index++;
        commands[index - 1].Execute();
        Debug.Log($"Redo: {commands[index - 1]}");
    }
}
