using System.Collections;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using ExcelData;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance => _instance;

    void Awake()
    {
        _instance = this;
    }

    public void SpawnMap(string path)
    {   
        GameObject mapNode = GameObject.Find(GameNode.BattleSceneNode.MapNode);
        AssetLoader.LoadAssetAsync<GameObject>(path, (mapAsset) => {
            GameObject mapClone = Instantiate(mapAsset, mapNode.transform);
            mapClone.transform.parent = mapNode.transform;
        });


        // UnityEngine.Object loadedObject = null;
        // StartCoroutine(_LoadAsync(path, out loadedObject));
    }

    public void SpawnUserLineup()
    {
        List<string> lineupData = LocalDataMgr.Instance.userLineup.userLineup;
        List<Transform> pos = LineUpMgr.Instance.playerLineup.position;

        for (int i = 0; i < LineUp.NUM; i++){
            string charKey = lineupData[i];
            string prefab = Character.GetItem(charKey).prefab;
            GameObject charPrefab = AssetDatabaseMgr.Instance.GetCharacter(prefab);
            GameObject charModel = Instantiate(charPrefab, pos[i]);
            charModel.transform.parent = pos[i];
        }
    }

    public void SpawnMonsterLineup()
    {
        var key = UICacheData.selectedStage;
        var stageCfg = Stage.GetItem(key);
        string[] lineupData = stageCfg.lineup.Split('+');
        List<Transform> pos = LineUpMgr.Instance.enemyLineup.position;

        for (int i = 0; i < LineUp.NUM; i++){
            string monsterKey = lineupData[i];
            if (!String.IsNullOrEmpty(monsterKey)){
                var prefab = Monster.GetItem(monsterKey).prefab;
                GameObject monsterPrefab = AssetDatabaseMgr.Instance.GetMonster(prefab);
                // Debuger.Log("monsterPrefab == null? " + (monsterPrefab == null) + " or pos[" + i + "] == null? " + (pos[i] == null));
                GameObject monsterModel = Instantiate(monsterPrefab, pos[i]);
                monsterModel.transform.parent = pos[i];
            }
        }
    }

}
