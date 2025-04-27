using System.Collections;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using UnityEngine;

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

	public bool IsDone {
		get {
			return (Progress == 1);
		}
	}


	public float Progress
	{
		get {
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

	public TaskAsync(AsyncOperation asyncOperation, string name, int id){
		this._asyncOperation = asyncOperation;
		this._name = name;
		this._id = id;
		this._type = Type.Task;
	}

	public TaskAsync(OperationSequence sequence, string name, int id){
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
		switch(_type){
			case Type.Task:
				yield return _asyncOperation; 
				break;
			case Type.Sequence:
				yield return _sequence._IEStart();
				break;
			case Type.Coroutine:
				yield return _coroutineStep.RunCoroutine();
				break;
			case Type.CoroutineSequence:
				yield return _coroutineSeq._IEStart();
				break;

		}
	}
}