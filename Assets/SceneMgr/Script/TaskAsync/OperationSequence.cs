using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationSequence
{
    private Dictionary<int, TaskAsync> tasks;
    private bool isStart;
    private int completedCount;
    private TaskAsync mCurrent;
    private bool isDone;
    private int hashcode;

    public int TaskCount => GetTaskCount();

    public bool IsDone => isDone;
    public float Progress => GetProgress();
    public float TaskProgress => GetTaskProgress();
    public string TaskName => GetTaskName();

    public int GetTaskCount()
    {
        if (tasks == null)
        {
            return 0;
        }
        return tasks.Count;
    }

    public override int GetHashCode()
    {
        return hashcode;
    }

    public OperationSequence()
    {
        tasks = new Dictionary<int, TaskAsync>();
        isStart = false;
        isDone = false;
        hashcode = DateTime.Now.Millisecond;
    }

    public string CurrentTaskName{
        get { 
            if (mCurrent == null){
                return "";
            }
            return mCurrent.Name;
        }
        private set{}
    }

    public float GetProgress()
    {
        if (TaskCount < 1){
            return 1;
        }
        
        if (completedCount == TaskCount){
            return 1;
        }

        float eachTaskPercent = 1f / TaskCount;
        float crrTaskProgress = mCurrent == null ? 0f : (float) mCurrent.Progress;
        return ((float)completedCount / TaskCount) + crrTaskProgress * eachTaskPercent;
    }

    private float GetTaskProgress()
    {
        if (!isStart) { return 0; }
        if (mCurrent == null) {  return 0; }
        return mCurrent.Progress;
    }

    private string GetTaskName()
    {
		if (!isStart) { return ""; }
		if (mCurrent == null) { return ""; }
		return mCurrent.Name;
	}

    //=======================================================\

    public void AddTask(OperationSequence sequence, string taskName){
        if (isStart){
            Debuger.Err("Can't add task once the Sequence has started!");
            return;
        }

        if (sequence.GetHashCode() == this.GetHashCode()){
            Debuger.Err("Can't add it self into its sub-task!");
            return;
        }

        TaskAsync taskAsync = new TaskAsync(sequence, taskName, sequence.GetHashCode());
        if (!tasks.ContainsKey(taskAsync.Id)){
            tasks.Add(taskAsync.Id, taskAsync);
        }
        else {
            Debuger.Err("The task: " + taskName + " was already Existed!");
        }
    }

    public void AddTask(AsyncOperation asyncOperation, string taskName){
        if (isStart){
            Debuger.Err("Can't add task once the Sequence has started!");
            return;
        }

        TaskAsync taskAsync = new TaskAsync(asyncOperation, taskName, asyncOperation.GetHashCode());
        if (!tasks.ContainsKey(taskAsync.Id)){
            tasks.Add(taskAsync.Id, taskAsync);
        }
        else {
            Debuger.Err("The task: " + taskName + " was already Existed!");
        }
    }

    public void AddTask(IEnumerator coroutine, string taskName)
    {
		if (isStart)
		{
			Debuger.Err("Can't add task once the Sequence has started!");
			return;
		}

		TaskAsync taskAsync = new TaskAsync(new CoroutineStep(coroutine, taskName), taskName, coroutine.GetHashCode());
		if (!tasks.ContainsKey(taskAsync.Id))
		{
			tasks.Add(taskAsync.Id, taskAsync);
		}
		else
		{
			Debuger.Err("The task: " + taskName + " was already Existed!");
		}
	}

	public void AddTask(CoroutineSequence coroutineSeq, string taskName)
	{
		if (isStart)
		{
			Debuger.Err("Can't add task once the Sequence has started!");
			return;
		}

		TaskAsync taskAsync = new TaskAsync(coroutineSeq, taskName, coroutineSeq.GetHashCode());
		if (!tasks.ContainsKey(taskAsync.Id))
		{
			tasks.Add(taskAsync.Id, taskAsync);
		}
		else
		{
			Debuger.Err("The task: " + taskName + " was already Existed!");
		}
	}

	public IEnumerator _IEStart()
    {
        if (isStart)
		{
			Debug.LogError("[OperationSequence] The sequence has been Started!!");
			yield break;
        }
        if (tasks == null || TaskCount == 0)
		{
			Debug.LogError("[OperationSequence] The sequence has NO Task!!");
			isDone = true;
            isStart = false;
            Debug.LogError("[OperationSequence] The sequence has Stopped!!");
            yield break;
        }
        else {
            Debug.Log("[OperationSequence] Sequence Start");
            isStart = true;
            isDone = false;
            completedCount = 0;

            yield return _IEStartSequence();

			isDone = true;
			isStart = false;
			Debug.Log("[OperationSequence] The sequence has Complete");
		}
    }

    IEnumerator _IEStartSequence()
	{
		var emunerator = tasks.GetEnumerator();

        while (emunerator.MoveNext()){
            var task = emunerator.Current;
            mCurrent = task.Value;

            if (mCurrent != null)
            {
                YellowCat.SceneMgr.CoroutineManager.Instance.StartCoroutine(mCurrent._IERun());
                while (!mCurrent.IsDone){
                    yield return null;
                }
            }
            
            completedCount++;
        }

        yield return null;
        isDone = true;
		isStart = false;
		Debug.Log("[OperationSequence] Sequence Done");
    }
}