using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loader thông minh, tự động tính % progress tổng thể cho chuỗi coroutine.
/// </summary>

namespace YellowCat.SceneMgr
{
	public class SmartCoroutine
	{
		public float Progress { get; private set; } = 0f;
		public bool IsDone { get; private set; } = false;
		public string CurrentTaskName { get; private set; } = "";

		private List<LoadingStep> steps = new List<LoadingStep>();
		private int currentStepIndex = 0;

		private IEnumerator _rootCoroutine;

		public SmartCoroutine(IEnumerator rootCoroutine)
		{
			_rootCoroutine = rootCoroutine;
		}

		// --- Public API ---
		public IEnumerator RunCoroutine()
		{
			Debuger.Log("[SmartCoroutine] RunCoroutine start...");
			IsDone = false;
			Progress = 0f;
			CurrentTaskName = "";

			Stack<IEnumerator> stack = new Stack<IEnumerator>();
			stack.Push(_rootCoroutine);

			int stepCount = 0;
			int totalStepsEstimate = 10; // Nếu muốn, có thể set từ ngoài.

			while (stack.Count > 0)
			{
				var current = stack.Pop();

				while (current.MoveNext())
				{
					var value = current.Current;

					if (value is IEnumerator sub)
					{
						stack.Push(current);
						Debuger.Log($"[SmartCoroutine] RunCoroutine > sub is {sub.GetType()}");
						current = sub;
						break;
					}
					else if (value is AsyncOperation asyncOp)
					{
						Debuger.Log($"[SmartCoroutine] RunCoroutine > sub is AsyncOperation");
						CurrentTaskName = $"Loading AsyncOperation";
						yield return RunAsyncOperation(asyncOp, stepCount, totalStepsEstimate);
						stepCount++;
					}
					else if (value is float f)
					{
						Debuger.Log($"[SmartCoroutine] RunCoroutine > sub is float");
						CurrentTaskName = $"Progress {f * 100f:0}%";
						Progress = Mathf.Clamp01((stepCount + f) / totalStepsEstimate);
						yield return null;
					}
					else if (value is WaitForSeconds)
					{
						Debuger.Log($"[SmartCoroutine] RunCoroutine > sub is WaitForSeconds");
						CurrentTaskName = value?.ToString() ?? $"Step {stepCount + 1}";
						Progress = Mathf.Clamp01((stepCount + 0f) / totalStepsEstimate);
						yield return value;
						stepCount++;
					}
					else
					{
						Debuger.Log($"[SmartCoroutine] RunCoroutine > sub is others");
						CurrentTaskName = value?.ToString() ?? $"Step {stepCount + 1}";
						Progress = Mathf.Clamp01((stepCount + 0f) / totalStepsEstimate);
						yield return value;
						stepCount++;
					}
				}
			}

			Progress = 0.9f;
			yield return null;
			yield return null;
			yield return null;

			Progress = 1f;
			IsDone = true;
			CurrentTaskName = "Done!";
			Debuger.Log("[SmartCoroutine] RunCoroutine > Done");
		}

		private IEnumerator RunAsyncOperation(AsyncOperation asyncOp, int stepCount, int totalSteps)
		{
			while (!asyncOp.isDone)
			{
				Progress = Mathf.Clamp01((stepCount + asyncOp.progress) / totalSteps);
				yield return null;
			}
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
}