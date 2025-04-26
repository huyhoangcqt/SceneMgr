using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
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

    public void Start()
    {
        if (isStart){
            return;
        }
        if (tasks == null || totalCount == 0)
        {
            isDone = true;
            isStart = false;
            return;
        }
        else {
            isStart = true;
            totalCount = tasks.Count;
            completedCount = 0;
            CoroutineManager.startCoroutine(_StartSequence());
        }
    }

    IEnumerator _StartSequence()
    {
        var emunerator = tasks.GetEnumerator();

        while (emunerator.MoveNext()){
            var task = emunerator.Current;
            mCurrent = task.Value;

            if (mCurrent != null){
                while (!mCurrent.IsDone){
                    yield return null;
                }

                completedCount++;
                yield return _StartSequence();
            }
        }

        yield return null;
        isDone = true;
        isStart = false;
    }
}