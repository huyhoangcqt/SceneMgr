using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
	public static GameObject Root {
		get
		{
			if (mRoot == null)
			{
				mRoot = new GameObject("Pools");
			}
			return mRoot;
		}
	}
	private static GameObject mRoot = null;
	private static Dictionary<string, Pool> poolMap = new Dictionary<string, Pool>();

	public static Pool GetPool(string poolName)
	{
		poolMap.TryGetValue(poolName, out Pool pool);
		return pool;
	}

	public static Pool GetPool(GameObject prefab)
	{
		return GetPool(prefab.name);
	}

	public static Pool CreatePool(GameObject prefab)
	{
		if (!poolMap.ContainsKey(prefab.name))
		{
			var poolRoot = new GameObject(prefab.name);
			poolRoot.transform.parent = Root.transform;
			poolMap.Add(prefab.name, new Pool(poolRoot, prefab));
		}
		return GetPool(prefab);
	}


	public static Pool CreatePool(GameObject prefab, int num)
	{
		if (!poolMap.ContainsKey(prefab.name))
		{
			var poolRoot = new GameObject(prefab.name);
			poolRoot.transform.parent = Root.transform;
			poolMap.Add(prefab.name, new Pool(poolRoot, prefab));
		}
		var pool = GetPool(prefab);
		pool.CreatePool(num);
		return pool;
	}


	public static GameObject GetObjectFromPool(string poolName)
	{
		var pool = GetPool(poolName);
		if (pool != null)
		{
			return pool.GetFromPool();
		}
		return null;
	}

	public static bool SaveObjectToPool(string poolName, GameObject prefab)
	{
		if (!poolMap.ContainsKey(poolName))
		{
			return false;
		}

		return poolMap[poolName].GiveBackToPool(prefab);
	}

	public static bool SaveAllObjectsToPool(string poolName)
	{
		if (!poolMap.ContainsKey(poolName))
		{
			return false;
		}

		poolMap[poolName].GiveBackToPoolAll();
		return true;
	}

	public static void ReleaseAll()
	{
		foreach (var pool in poolMap.Values)
			pool.ReleaseAll();
		poolMap.Clear();
	}
}
