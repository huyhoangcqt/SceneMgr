
using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool<T>
{
	private Dictionary<T, GameObject> objectList = new Dictionary<T, GameObject>();
	private Transform mRoot;
	private Pool mPool;

	public GameObjectPool(Transform root, Pool pool)
	{
		this.mRoot = root;
		this.mPool = pool;
	}

	public GameObject GetFromPool()
	{
		Debug.Log("[GameObjectPool] > GetFromPool");
		return mPool.GetFromPool();
	}

	public void ReleaseAllToPool()
	{
		Debug.Log("[GameObjectPool] > ReleaseAllToPool");
		foreach (var obj in objectList.Values)
		{
			mPool.GiveBackToPool(obj);
		}
		objectList.Clear();
	}

	public void AddObject(GameObject go, T index)
	{
		if (objectList.ContainsKey(index))
		{
			go.transform.SetParent(mRoot);
			go.SetActive(true);
			objectList[index] = go;
			return;
		}
		go.transform.SetParent(mRoot);
		go.SetActive(true);
		objectList.Add(index, go);
	}

	public bool IsContainAt(T index)
	{
		return objectList.ContainsKey(index);
	}

	public GameObject GetObject(T index)
	{
		if (objectList.ContainsKey(index))
		{
			return objectList[index];
		}
		return null;
	}

	public bool RemoveObject(T index)
	{
		if (!objectList.ContainsKey(index))
		{
			Debug.LogWarning($"[LayerObject] the objectList is not containsKey({index})");
			return false;
		}
		GameObject obj = objectList[index];
		obj.SetActive(false);
		mPool.GiveBackToPool(obj);
		objectList.Remove(index);
		return true;
	}
}
