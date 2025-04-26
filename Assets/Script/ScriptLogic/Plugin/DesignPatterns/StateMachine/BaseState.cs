using UnityEngine;

public class BaseState
{
    public void Enter()
    {
        onEnter();
    }

    protected virtual void onEnter()
    {

    }

    public void Leave()
    {
        onLeave();
    }

    protected virtual void onLeave()
    {
        
    }
}