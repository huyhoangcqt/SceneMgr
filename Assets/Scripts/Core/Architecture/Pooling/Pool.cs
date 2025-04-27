using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class Pool
{
    private const int DEFAULT_NUM = 10;
    private int num = -1;

    private GameObject mRoot;
    private GameObject instance;
    public string Name => instance.name;

    private Dictionary<int, GameObject> pools;
    private Dictionary<int, GameObject> activeList;


    public Pool(GameObject root, GameObject prefab)
    {
        this.mRoot = root;
        this.num = DEFAULT_NUM;
        this.instance = prefab;
        this.pools = new Dictionary<int, GameObject>();
        this.activeList = new Dictionary<int, GameObject>();
    }

    public Pool(GameObject root, GameObject prefab, int num)
	{
		this.mRoot = root;
		this.num = num;
        this.instance = prefab;
        this.pools = new Dictionary<int, GameObject>();
        this.activeList = new Dictionary<int, GameObject>();
    }

    public void CreatePool()
    {
        int needCreateNum = this.num - pools.Count;
        for (int i = 0; i < needCreateNum; i++)
        {
            var item = GameObject.Instantiate(instance);
            item.transform.SetParent(mRoot.transform);
            item.gameObject.SetActive(false);
            pools.Add(item.GetHashCode(), item);
        }
    }

    public void CreatePool(int num)
    {
        this.num = num;
        CreatePool();
    }

    public GameObject GetLast()
    {
        var item = pools.Values.Last();
        return item;
    }

    public GameObject GetFromPool()
	{
		Debug.Log("Pool > GetFromPool");
		if (pools == null || pools.Count == 0)
        {
            this.num += DEFAULT_NUM;
            CreatePool();
        }

        var item = GetLast();

		int hashCode = item.GetHashCode();
        pools.Remove(hashCode);
        activeList.TryAdd(hashCode, item);
        return item;
    }

    public void GiveBackToPool(List<GameObject> listItem)
    {
        Debug.Log("Pool > GiveBackToPool > itemCount: " + listItem.Count);

		foreach (var item in listItem)
        {
            GiveBackToPool(item);
        }
    }

    public bool GiveBackToPool(GameObject item)
    {
        Debug.Log("GiveBackToPool > item: " + item.name);
        int hashCode = item.GetHashCode();
        activeList.Remove(hashCode);
        if (pools.TryAdd(hashCode, item))
        {
            item.gameObject.SetActive(false);
            item.transform.SetParent(mRoot.transform);
            return true;
        }
        return false;
    }

    public void GiveBackToPoolAll()
    {
        var activeItems = activeList.Values.ToList();
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var item = activeItems[i];
            GiveBackToPool(item);
        }
    }

    public void ReleaseAll()
    {
        var activeItems = activeList.Values.ToList();
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var item = activeItems[i];
            activeList.Remove(item.GetHashCode());
            GameObject.DestroyImmediate(item);
        }

        var poolItems = pools.Values.ToList();
        for (int i = pools.Count - 1; i >= 0; i--)
        {
            var item = poolItems[i];
            pools.Remove(item.GetHashCode());
            GameObject.DestroyImmediate(item);
        }
    }
}
