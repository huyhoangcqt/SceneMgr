using OfficeOpenXml.ConditionalFormatting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandManager
{
	public void AddCmd(ICommand cmd);
	public void ExecuteCmd();
	public void Undo();
	public void Redo();
}

public class CommandManager : ICommandManager
{
	public Queue<ICommand> cmdQueue = new Queue<ICommand>();
	public Stack<ICommand> undoStack = new Stack<ICommand>();
	public Stack<ICommand> redoStack = new Stack<ICommand>();

	public CommandManager()
	{
		cmdQueue = new Queue<ICommand>();
		undoStack = new Stack<ICommand>();
		redoStack = new Stack<ICommand>();
	}

	public void AddCmd(ICommand cmd)
	{
		if (!cmdQueue.Contains(cmd))
		{
			cmdQueue.Enqueue(cmd);
		}
		else
		{
			Debug.LogWarning("The Command has been Existed!");
		}
	}

	public void ExecuteCmd()
	{
		if (cmdQueue.Count > 0)
		{
			var cmd = cmdQueue.Dequeue();
			cmd.Execute();
			undoStack.Push(cmd);
			redoStack.Clear();
		}
	}

	public void Undo()
	{
		if (undoStack.Count > 0) 
		{
			var cmd = undoStack.Pop();
			cmd.Undo();
			redoStack.Push(cmd);
		}
	}

	public void Redo()
	{
		if (redoStack.Count > 0)
		{
			var cmd = redoStack.Pop();
			cmd.Execute();
			undoStack.Push(cmd);
		}
	}
}
