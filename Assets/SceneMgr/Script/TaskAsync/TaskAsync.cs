using System.Collections;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class TaskAsync
	{
		public enum Type
		{
			Task,
			Sequence,
			Coroutine,
			CoroutineSequence,
		}

		private int _id;
		private string _name;
		private AsyncOperation _asyncOperation;
		private OperationSequence _sequence;
		private CoroutineStep _coroutineStep;
		private CoroutineSequence _coroutineSeq;
		private Type _type;
		private bool _isDone = false;

		public bool IsDone => _isDone;


		public float Progress
		{
			get
			{
				switch (_type)
				{
					case Type.Task:
						return _asyncOperation.progress;
					case Type.Sequence:
						return _sequence.Progress;
					case Type.Coroutine:
						return _coroutineStep.Progress;
					case Type.CoroutineSequence:
						return _coroutineSeq.Progress;
				}
				return 0f;
			}
		}

		public string Name => _name;
		public int Id => _id;
		public Type TaskType => _type;

		public override int GetHashCode()
		{
			switch (_type)
			{
				case Type.Task:
					return _asyncOperation.GetHashCode();
				case Type.Sequence:
					return _sequence.GetHashCode();
				case Type.Coroutine:
					return _coroutineStep.GetHashCode();
				case Type.CoroutineSequence:
					return _coroutineSeq.GetHashCode();
			}
			return -1;
		}

		public TaskAsync(AsyncOperation asyncOperation, string name, int id)
		{
			this._asyncOperation = asyncOperation;
			this._name = name;
			this._id = id;
			this._type = Type.Task;
		}

		public TaskAsync(OperationSequence sequence, string name, int id)
		{
			this._sequence = sequence;
			this._name = name;
			this._id = id;
			this._type = Type.Sequence;
		}

		public TaskAsync(CoroutineStep coroStep, string name, int id)
		{
			this._coroutineStep = coroStep;
			this._name = name;
			this._id = id;
			this._type = Type.Coroutine;
		}

		public TaskAsync(CoroutineSequence coroSequence, string name, int id)
		{
			this._coroutineSeq = coroSequence;
			this._name = name;
			this._id = id;
			this._type = Type.CoroutineSequence;
		}

		public IEnumerator _IERun()
		{
			_isDone = false;

			Debuger.Log($"[OperationSequence] Task '{_name}' Start");
			switch (_type)
			{
				case Type.Task:
					yield return new WaitUntil(() => { return _asyncOperation.isDone; });
					break;
				case Type.Sequence:
					YellowCat.SceneMgr.CoroutineManager.Instance.StartCoroutine(_sequence._IEStart());
					yield return new WaitUntil(() => { return _sequence.IsDone; });
					break;
				case Type.Coroutine:
					YellowCat.SceneMgr.CoroutineManager.Instance.StartCoroutine(_coroutineStep.RunCoroutine());
					yield return new WaitUntil(() => { return _coroutineStep.IsDone; });
					break;
				case Type.CoroutineSequence:
					YellowCat.SceneMgr.CoroutineManager.Instance.StartCoroutine(_coroutineSeq._IEStart());
					yield return new WaitUntil(() => { return _coroutineSeq.IsDone; });
					break;
			}

			_isDone = true;
			Debuger.Log($"[OperationSequence] Task '{_name}' Done");
		}
	}
}