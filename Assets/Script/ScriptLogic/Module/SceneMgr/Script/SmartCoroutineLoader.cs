using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loader thông minh, tự động tính % progress tổng thể cho chuỗi coroutine.
/// </summary>
public class SmartCoroutineLoader
{
	public float Progress { get; private set; } = 0f;
	public bool IsDone { get; private set; } = false;
	public string CurrentTaskName { get; private set; } = "";

	private List<LoadingStep> steps = new List<LoadingStep>();
	private int currentStepIndex = 0;

	private IEnumerator _rootCoroutine;

	public SmartCoroutineLoader(IEnumerator rootCoroutine)
	{
		_rootCoroutine = rootCoroutine;
	}

	// --- Public API ---
	public IEnumerator RunCoroutine()
	{
		Debug.Log("[SmartCoroutineLoader] RunCoroutine start...");
		IsDone = false;
		steps.Clear();
		currentStepIndex = 0;

		yield return ExtractSteps(_rootCoroutine);

		while (currentStepIndex < steps.Count)
		{
			var step = steps[currentStepIndex];

			CurrentTaskName = step.TaskName ?? $"Step {currentStepIndex + 1}/{steps.Count}";

			if (step.Action is IEnumerator subCoroutine)
			{
				yield return RunSubCoroutine(subCoroutine);
			}
			else if (step.Action is AsyncOperation asyncOp)
			{
				yield return RunAsyncOperation(asyncOp);
			}
			else
			{
				if (step.Action is float f)
				{
					Progress = Mathf.Clamp01((currentStepIndex + f) / steps.Count);
				}
				else
				{
					Progress = Mathf.Clamp01((currentStepIndex + 0f) / steps.Count);
				}
				yield return null;
			}

			currentStepIndex++;
		}

		Progress = 1f;
		IsDone = true;
		CurrentTaskName = "Done!";
		Debug.Log("[SmartCoroutineLoader] RunCoroutine > Done");
	}

	// --- Internal ---

	private IEnumerator RunSubCoroutine(IEnumerator coroutine)
	{
		while (coroutine.MoveNext())
		{
			if (coroutine.Current is float f)
			{
				Progress = Mathf.Clamp01((currentStepIndex + f) / steps.Count);
			}
			else if (coroutine.Current is AsyncOperation op)
			{
				yield return RunAsyncOperation(op);
			}
			else if (coroutine.Current is IEnumerator sub)
			{
				yield return RunSubCoroutine(sub);
			}
			else
			{
				Progress = Mathf.Clamp01((currentStepIndex + 0f) / steps.Count);
				yield return null;
			}
		}
	}

	private IEnumerator RunAsyncOperation(AsyncOperation asyncOp)
	{
		while (!asyncOp.isDone)
		{
			Progress = Mathf.Clamp01((currentStepIndex + asyncOp.progress) / steps.Count);
			yield return null;
		}
	}

	private IEnumerator ExtractSteps(IEnumerator coroutine)
	{
		Stack<IEnumerator> stack = new Stack<IEnumerator>();
		stack.Push(coroutine);

		while (stack.Count > 0)
		{
			var current = stack.Pop();
			while (current.MoveNext())
			{
				var value = current.Current;

				if (value is IEnumerator sub)
				{
					stack.Push(current);
					current = sub;
				}
				else
				{
					steps.Add(new LoadingStep(value, TryExtractTaskName(value)));
				}
			}
		}

		yield return null;
	}

	private string TryExtractTaskName(object action)
	{
		if (action == null) return null;
		if (action is AsyncOperation asyncOp)
		{
			return $"AsyncOperation ({asyncOp.ToString()})";
		}
		if (action is IEnumerator coroutine)
		{
			return $"Coroutine ({coroutine.ToString()})";
		}
		if (action is float f)
		{
			return $"Progress {f * 100f:0}%";
		}
		return action.ToString();
	}

	private class LoadingStep
	{
		public object Action;
		public string TaskName;

		public LoadingStep(object action, string taskName)
		{
			Action = action;
			TaskName = taskName;
		}
	}
}
