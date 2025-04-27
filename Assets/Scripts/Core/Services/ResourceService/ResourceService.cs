using OfficeOpenXml.ConditionalFormatting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceService : MonoSingletonTemplate<ResourceService>, IResourceService
{
	//Cached
	Dictionary<string, Sprite> spriteCached = new Dictionary<string, Sprite>();
	Dictionary<string, GameObject> prefabCached = new Dictionary<string, GameObject>();	

	//Private
	private Sprite GetSprite(string fullpath)
	{
		if (spriteCached.ContainsKey(fullpath))
		{
			return spriteCached[fullpath];
		}

		var result = Resources.Load<Sprite>(fullpath);
		if (result == null)
		{
			Debug.LogWarning($"The sprite: {fullpath} was NOT FOUND!");
			return null;
		}
		else
		{
			//Get the sprite
			spriteCached.Add(fullpath, result);
			return result;
		}

	}

	private GameObject GetPrefab(string fullpath)
	{
		if (prefabCached.ContainsKey(fullpath))
		{
			return prefabCached[fullpath];
		}


		var result = Resources.Load<GameObject>(fullpath);
		if (result == null)
		{
			Debug.LogWarning($"The sprite: {fullpath} was NOT FOUND!");
			return null;
		}
		else
		{
			//Get the sprite
			prefabCached.Add(fullpath, result);
			return result;
		}
	}

	//Public

	public Sprite GetSprite(string package, string name)
	{
		string fullPath = $"{package}/{name}";
		return GetSprite(fullPath);
	}



	public GameObject GetPrefab(string package, string prefabName)
	{
		string fullPath = $"{package}/{prefabName}";
		return GetPrefab(fullPath);
	}
}
