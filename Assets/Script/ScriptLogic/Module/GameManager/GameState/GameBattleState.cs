using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameBattleState : BaseState
{
    protected override void onEnter()
    {
        Debuger.Log("GameBattleState Enter");
        SceneMgr.Instance.ChangeSceneAsync(Scene.BattleScene, () => {
            CoroutineManager.startCoroutine(_PreLoadAssets());
        });
    }

    IEnumerator _PreLoadAssets()
    {
        AssetDatabaseMgr.Instance.LoadAllCharacters();
        yield return null;
        yield return null;
        AssetDatabaseMgr.Instance.LoadAllMonster();
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return _LoadMapAndMonster();
        yield return null;
    }

    IEnumerator _LoadMapAndMonster()
    {
        string mapAssetPath = "Assets/HotupdateAssets/Prefabs/Map/Map_001.prefab";
        SpawnManager.Instance.SpawnMap(mapAssetPath);
        yield return null;
        SpawnManager.Instance.SpawnUserLineup();
        yield return null;
        SpawnManager.Instance.SpawnMonsterLineup();
        yield return null;
    }
    
    protected override void onLeave()
    {
        Debuger.Log("GameBattleState Leave");
    }
}