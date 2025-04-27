using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cooldown
{
    private float duration;
    private float remain;
    private bool isCooldown = false;
    private bool isReady = false;
    private Action<float> onUpdate;
    private Action onComplete;

    //public
    public float Duration { get { return duration; } }
    public float RemainTime { get { return remain; } }
    public bool IsCooldown { get { return isCooldown; } }
    public bool IsReady { get { return isReady; } } 

    public Cooldown(float duration, Action<float> onUpdate, Action onComplete)
    {
        this.duration = duration;
        this.remain = duration;
        this.onUpdate = onUpdate;
        this.onComplete = onComplete;
    }

    public void StartCountdown()
    {
        isCooldown = true;
        isReady = false;
    }

    public void Countdown(float delta)
    {
        if (!isCooldown || isReady)
        {
            return;
        }

        remain -= delta;
        if (remain > 0)
        {
            onUpdate?.Invoke(remain);
        }

        if (remain <= 0)
        {
            isReady = true;
            DoComplete();
        }
    }

    private void DoComplete()
    {
        onComplete?.Invoke();
    }

    public void Stop()
    {
        onComplete = null;
        onUpdate = null;
    }
}
