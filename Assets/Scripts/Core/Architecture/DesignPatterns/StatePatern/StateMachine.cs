using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachine<T>
{
	private Dictionary<T, BaseState> mStates;
	private BaseState mCrrState = null;
	private T mCrrKey;

	public StateMachine(Dictionary<T, BaseState> states, T startState)
	{
		this.mStates = new Dictionary<T, BaseState>();
		this.mStates.AddRange(states);
		this.mCrrKey = startState;

		if (!states.ContainsKey(startState))
		{
			Debug.LogError("Failed to Init default states: " + startState);
			return;
		}

		mCrrState = states[startState];
		mCrrState?.Enter();
	}

	public void ChangeState(T state)
	{
		if (!mStates.ContainsKey(state))
		{
			Debug.LogError($"The State: {state.ToString()} is not Registered!");
		}
		else
		{
			mCrrState.Exit();
			mCrrKey = state;
			mCrrState = mStates[mCrrKey];
			mCrrState.Enter();
		}
	}
}