using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CoroutineSequence
{
	private Dictionary<int, CoroutineStep> _steps = new Dictionary<int, CoroutineStep>();
	private CoroutineStep mCurrent;

	private bool _isStart = false;
	private float _progress = 0f;
	private bool _isDone = false;
	private int _totalCount = 0;
	private int _completedCount = 0;

	public bool IsStart { get => _isStart; private set {} }
	public float Progress { get => _progress; private set { } }
	public bool IsDone { get => _isDone; private set { } }
	public int TotalCount { get => _totalCount; private set { } }
	public int CompletedCount { get => _completedCount; private set { } }

	public CoroutineSequence()
	{
		_steps = new Dictionary<int, CoroutineStep>();
		_isStart = false;
	}

	public void AddStep(CoroutineStep step)
	{
		if (_isStart)
		{
			Debug.LogWarning("Add Step failed duel to Sequence has been Started!");
		}
		else
		{
			_steps.Add(_steps.Count, step);
		}
	}

	public IEnumerator _IEStart()
	{
		if (_isStart)
		{
			yield break;
		}

		if (_steps == null || _steps.Count == 0)
		{
			_isDone = true;
			_isStart = false;
			_progress = 1f;
			yield break;
		}
		else
		{
			_isStart = true;
			_totalCount = _steps.Count;
			_completedCount = 0;
			yield return _IEStartSequence();
		}
	}

	IEnumerator _IEStartSequence()
	{
		var emunerator = _steps.GetEnumerator();

		while (emunerator.MoveNext())
		{
			var task = emunerator.Current;
			mCurrent = task.Value;

			if (mCurrent != null)
			{
				mCurrent.RunCoroutine();
				while (!mCurrent.IsDone)
				{
					yield return null;
				}

				_completedCount++;
				_progress = _completedCount / _totalCount;

				yield return _IEStartSequence();
			}
		}

		yield return null;
		_progress = 1f;
		_isDone = true;
		_isStart = false;
	}

}