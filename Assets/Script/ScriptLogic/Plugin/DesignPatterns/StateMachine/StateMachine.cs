using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine : Singleton<StateMachine>
{
    private Dictionary<string, BaseState> mStates;
    BaseState mCurrent = null;

    public StateMachine() : base()
    {
        Init();
    }

    private void Init()
    {
        mStates = new Dictionary<string,BaseState>();
    }

    public void ChangeState(string name)
    {
        if (mStates.ContainsKey(name))
        {
            if (mCurrent != null)
            {
                mCurrent.Leave();
            }
            mCurrent = mStates[name];
            mCurrent.Enter();
        }
        else {
            Debug.LogError("This state machine don't register the state: " + name);
        }
    }

    public void RegisterState(string name, BaseState state)
    {
        if (!mStates.ContainsKey(name)){
            mStates.Add(name, state);
        }
    }

    public void RemoveState(string name){
        if (mStates.ContainsKey(name)){
            mStates.Remove(name);
        }   
    }
}

public static class StateMachineExtension
{
    //GameState
    public static void RegisterState(this StateMachine machine, GameState name, BaseState state)
    {
        machine.RegisterState(name.ToString(), state);
    }

    public static void RemoveState(this StateMachine machine, GameState name)
    {
        machine.RemoveState(name.ToString());
    }

    public static void ChangeState(this StateMachine machine, GameState name)
    {
        machine.ChangeState(name.ToString());
    }

    //Scene
    // public static void RegisterState(this StateMachine machine, Scene name, BaseState state)
    // {
    //     machine.RegisterState(name.ToString(), state);
    // }

    // public static void RemoveState(this StateMachine machine, Scene name)
    // {
    //     machine.RemoveState(name.ToString());
    // }

    // public static void ChangeState(this StateMachine machine, Scene name)
    // {
    //     machine.ChangeState(name.ToString());
    // }
}