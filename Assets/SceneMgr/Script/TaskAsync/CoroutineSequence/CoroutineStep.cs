using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YellowCat.SceneMgr;

public class CoroutineStep
{
	private SmartCoroutine _enumerator;
	private string _taskName;
	private float _progress;
	private bool _isDone = false;

	public string TaskName { get => _taskName; private set { } }
	public float Progress { get => _progress; private set { } }
	public bool IsDone { get => _isDone; private set { } }

	public CoroutineStep(IEnumerator enumerator, string taskName)
	{
		this._enumerator = new SmartCoroutine(enumerator);
		this._taskName = taskName;
	}

	public IEnumerator RunCoroutine()
	{
		_progress = 0f;
		YellowCat.SceneMgr.CoroutineManager.Instance.StartCoroutine(_enumerator.RunCoroutine());
		while (!_enumerator.IsDone)
		{
			_progress = _enumerator.Progress;
			yield return null;
		}

		yield return null;
		yield return null;

		_progress = 1f;
		_isDone = true;
	}
}
