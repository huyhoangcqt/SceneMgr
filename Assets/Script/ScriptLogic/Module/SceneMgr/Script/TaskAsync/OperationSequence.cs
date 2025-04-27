using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationSequence
{
    private Dictionary<int, TaskAsync> tasks;
    private bool isStart;
    private int completedCount;
    private int totalCount;
    private TaskAsync mCurrent;
    private bool isDone;
    public bool IsDone => isDone;
    public float Progress => GetProgress();
    private int hashcode;
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

    private float GetProgress()
    {
        if (totalCount < 1){
            return 1;
        }
        
        if (completedCount == totalCount){
            return 1;
        }

        float eachTaskPercent = 1 / totalCount;
        float crrTaskProgress = mCurrent == null ? 0 : (float) mCurrent.Progress;
        return (completedCount * eachTaskPercent) + crrTaskProgress * eachTaskPercent;
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
        if (isStart){
            yield break;
        }
        if (tasks == null || totalCount == 0)
        {
            isDone = true;
            isStart = false;
            yield break;
        }
        else {
            isStart = true;
            totalCount = tasks.Count;
            completedCount = 0;
            yield return _IEStartSequence();
        }
    }

    IEnumerator _IEStartSequence()
    {
        var emunerator = tasks.GetEnumerator();

        while (emunerator.MoveNext()){
            var task = emunerator.Current;
            mCurrent = task.Value;
            mCurrent._IERun();

            if (mCurrent != null){
                while (!mCurrent.IsDone){
                    yield return null;
                }

                completedCount++;
                yield return _IEStartSequence();
            }
        }

        yield return null;
        isDone = true;
        isStart = false;
    }
}