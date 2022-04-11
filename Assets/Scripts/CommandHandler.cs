using System.Collections.Generic;
using UnityEngine;

public static class CommandHandler
{
    private static readonly List<ICommand> commands = new List<ICommand>();
    private static int index;

    public static void Add(ICommand command)
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

    public static void Undo()
    {
        if (commands.Count == 0) return;
        if (index <= 0) return;
        commands[index - 1].Undo();
        Debug.Log($"Undo: {commands[index - 1]}");
        index--;
    }
    
    public static void Redo()
    {
        if (commands.Count == 0) return;
        if (index >= commands.Count) return;
        index++;
        commands[index - 1].Execute();
        Debug.Log($"Redo: {commands[index - 1]}");
    }
}
