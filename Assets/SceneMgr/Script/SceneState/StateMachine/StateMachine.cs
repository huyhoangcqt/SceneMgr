using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class StateMachine<T>
	{
		private Dictionary<T, IState> mStates;
		private IState mCrrState = null;
		private T mCrrKey;

		public StateMachine(Dictionary<T, IState> states, T startState)
		{
			this.mStates = new Dictionary<T, IState>();
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

		public bool ChangeState(T state)
		{
			if (!mStates.ContainsKey(state))
			{
				Debug.LogError($"The State: {state.ToString()} is not Registered!");
				return false;
			}
			else
			{
				mCrrState.Exit();
				mCrrKey = state;
				mCrrState = mStates[mCrrKey];
				mCrrState.Enter();
				return true;
			}
		}
	}


}
