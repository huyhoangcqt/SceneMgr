using UnityEngine;
using System;
using System.Collections;
using YellowCat.SceneMgr;

public class BattleSceneController : MonoBehaviour, ISceneLoadable
{
	public IEnumerator LoadAsync()
	{
		Debug.Log("[BattleSceneController] LoadAsync...");
		yield return _PreLoadAssets();
		yield return null;

		yield return _LoadMap();
		yield return null;

		yield return _LoadMonster();
		yield return null;


		Debug.Log("[BattleSceneController] LoadAsync > Done");
		yield return 1f; // Tiến trình 100%
	}


	IEnumerator _PreLoadAssets()
	{
		Debug.Log("[BattleSceneController] _PreLoadAssets...");
		AssetDatabaseMgr.Instance.LoadAllCharacters();
		yield return new WaitForSeconds(0.1f);
		AssetDatabaseMgr.Instance.LoadAllMonster();
		yield return new WaitForSeconds(0.2f);
	}

	IEnumerator _LoadMap()
	{
		Debug.Log("[BattleSceneController] _LoadMap...");
		string mapAssetPath = "Assets/HotupdateAssets/Prefabs/Map/Map_001.prefab";
		SpawnManager.Instance.SpawnMap(mapAssetPath);
		Debug.Log("[BattleSceneController] _LoadMap > Done");
		yield return null;
	}

	IEnumerator _LoadMonster()
	{
		Debug.Log("[BattleSceneController] _LoadMonster...");
		SpawnManager.Instance.SpawnUserLineup();
		yield return null;
		SpawnManager.Instance.SpawnMonsterLineup(); 
		Debug.Log("[BattleSceneController] _LoadMonster > Done");
		yield return null;
	}
}
