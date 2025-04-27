using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineStep
{
	private IEnumerator _enumerator;
	private string _taskName;
	private float _progress;
	private bool _isDone = false;

	public string TaskName { get => _taskName; private set { } }
	public float Progress { get => _progress; private set { } }
	public bool IsDone { get => _isDone; private set { } }


	public CoroutineStep(IEnumerator enumerator, string TaskName)
	{
		this._enumerator = enumerator;
		this.TaskName = TaskName;
	}


	public IEnumerator RunCoroutine()
	{
		Progress = 0f;
		yield return _enumerator;
		Progress = 1f;
		IsDone = true;
	}
}
