using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceService
{
	public Sprite GetSprite(string package, string spriteName);

	public GameObject GetPrefab(string package, string prefabName);
}
