using System;
using System.Collections.Generic;
using UnityEngine;

public class Dispatcher<T>
{
	private Dictionary<T, Action<object>> mCallbacks = new Dictionary<T, Action<object>>();

	public void RegisterCallback(T name, Action<object> callback)
	{
		Debug.Log("[Dispatcher] RegisterCallback > name: " + name.ToString());
		if (!mCallbacks.ContainsKey(name))
		{
			mCallbacks.Add(name, callback);
		}
		else 
		{ 
			mCallbacks[name] += callback;
		}
	}

	public void UnregisterCallback(T name, Action<object> callback)
	{
		Debug.Log("[Dispatcher] UnregisterCallback > name: " + name.ToString());
		if (mCallbacks.ContainsKey(name)) {
			mCallbacks[name] -= callback;
			if (mCallbacks[name] == null)
			{
				mCallbacks.Remove(name);
			}
		}
	}

	public void InvokeEvent(T name, object data)
	{
		if (mCallbacks.ContainsKey(name))
		{
			Debug.Log($"[Dispatcher] InvokeEvent > name: {name.ToString()} > length: {mCallbacks[name].GetInvocationList().Length}");
			mCallbacks[name]?.Invoke(data);
		}
	}
}


/*
-------------------------------------------
------------ USING: SAMPLE ----------------
-------------------------------------------

public enum NetEvent
{

}

public enum GameEvent
{

}


public class EscapeEventManager
{
	private static Dispatcher GED = new Dispatcher();
	private static Dispatcher NED = new Dispatcher();

	//------------- GED
	public static void RegisterEvent(GameEvent ev, Action<object> callback)
	{
		GED.RegisterCallback(ev.ToString(), callback);
	}

	public static void UnregisterEvent(GameEvent ev, Action<object> callback) 
	{
		GED.UnregisterCallback(ev.ToString(), callback);
	}

	public static void DispatcherEvent(GameEvent ev, object data)
	{
		GED.InvokeEvent(ev.ToString(), data);
	}


	//------------- NED
	public static void RegisterEvent(NetEvent ev, Action<object> callback)
	{
		NED.RegisterCallback(ev.ToString(), callback);
	}

	public static void UnregisterEvent(NetEvent ev, Action<object> callback)
	{
		NED.UnregisterCallback(ev.ToString(), callback);
	}

	public static void DispatcherEvent(NetEvent ev, object data)
	{
		NED.InvokeEvent(ev.ToString(), data);
	}
}


 */