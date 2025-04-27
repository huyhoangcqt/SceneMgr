using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public interface ICooldownService
{
	/// <summary>
	/// --->> Return Cooldown KEY <<---
	/// </summary>
	/// <param name="duration"></param>
	/// <returns></returns>
	public int AddCooldown(float duration, Action<float> _UpdateCallback, Action _CompleteCallback);


	public float GetRemainCooldownTime(int key);

	public void RemoveCooldown(int key);

	public void Dipose();
}


public class CooldownManager : MonoBehaviour, ICooldownService
{
	//STATIC
	private static int _KEY = 0;

	private static int getKey()
	{
		_KEY ++;
		return _KEY;
	}

	private static void ResetKey()
	{
		_KEY = 0;
	}

	//attributes
	private Dictionary<int, Cooldown> cooldownList = new Dictionary<int, Cooldown>();


	//FUNCTION



	public int AddCooldown(float duration, Action<float> _UpdateCallback, Action _CompleteCallback)
	{
		var key = getKey();

		System.Action onCompleteWrapper = () =>
		{
			_CompleteCallback?.Invoke();
			DoComplete(key);
		};

		var cd = new Cooldown(duration, _UpdateCallback, onCompleteWrapper);
		if (!cooldownList.ContainsKey(key))
		{
			cooldownList.Add(key, cd);
			cd.StartCountdown();
			return key;
		}
		return -1;
	}

	public void DoComplete(int key)
	{
		cooldownList.Remove(key);
		Debug.Log($"Cooldown [{key}] completed & removed.");
	}



	public float GetRemainCooldownTime(int key)
	{
		if (!cooldownList.ContainsKey(key))
		{
			return -1;
		}
		return cooldownList[key].RemainTime;
	}

	public void RemoveCooldown(int key)
	{
		if (cooldownList.ContainsKey(key))
		{
			cooldownList[key].Stop();
			cooldownList.Remove(key);
		}

	}

	public void Dipose()
	{
		foreach (var cooldown in cooldownList.Values)
		{
			cooldown.Stop();
		}
		cooldownList.Clear();
		ResetKey();
	}
}
