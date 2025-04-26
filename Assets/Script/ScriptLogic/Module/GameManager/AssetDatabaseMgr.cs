using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExcelData;

public class AssetDatabaseMgr : Singleton<AssetDatabaseMgr>
{
    /**
    We need to save cache prefab here:
        1. Những asset cần sử dụng nhiều lần: như những hero mà user đã sở hữu/ unlock
        2. Những monster xuất hiện trong các chapter đã unlock/ load nguyên monster package.
        3. UI atlas
        4. OnLoad mainScene => cần tải hết package trong main scene rồi mới Khởi tạo được mainScene, quá trình process sẽ hiển thị trên LoadingWindow.
        => hoàn tất thì mới show mainScene.
        5. Tương tự cho quá trình load BattleScene.
        6. Trong quá trình hoạt động ở mainScene => Có thể tranh thủ chạy ngầm kéo các asset của BattleScene về trước.
    */
    
    public const string CHARACTER_PATH = "Assets/HotupdateAssets/Prefabs/Characters/";
    public const string MONSTER_PATH = "Assets/HotupdateAssets/Prefabs/Monster/";
    public const string MAP_PATH = "Assets/HotupdateAssets/Prefabs/Map/";

    Dictionary<string, GameObject> mCharacters;
    Dictionary<string, GameObject> mMonsters;

    public string GetCharacterPath(string prefabName){
        return CHARACTER_PATH + prefabName;
    }
    public string GetMonsterPath(string prefabName){
        return MONSTER_PATH + prefabName;
    }

    public AssetDatabaseMgr() : base()
    {
        Initialization();
    }

    private void Initialization()
    {
        mCharacters = new Dictionary<string, GameObject>();
        mMonsters = new Dictionary<string, GameObject>();
    }

    public void LoadAllCharacters()
    {        
        var charCfgs = Character.GetDict();
        var enumerator = charCfgs.GetEnumerator();
        while (enumerator.MoveNext()){
            var charCfg = enumerator.Current.Value;
            if (charCfg != null){
                string charPrefab = charCfg.prefab;
                string path = GetCharacterPath(charPrefab);
                if (!mCharacters.ContainsKey(path)){
                    AssetLoader.LoadAssetAsync<GameObject>(path, (go) => {
                        mCharacters.Add(path, go);
                    });
                }
            }
        }
    }

    public void LoadAllMonster()
    {
        var monsterCfgs = Monster.GetDict();
        var enumerator = monsterCfgs.GetEnumerator();
        while (enumerator.MoveNext()){
            var monsterCfg = enumerator.Current.Value;
            if (monsterCfg != null){
                string monsterPrefab = monsterCfg.prefab;
                string path = GetMonsterPath(monsterPrefab);
                if (!mMonsters.ContainsKey(path)){
                    AssetLoader.LoadAssetAsync<GameObject>(path, (go) => {
                        mMonsters.Add(path, go);
                    });
                }
            }
        }
    }

    // #region Get SequenceLoading
    /*\
    // NOTE Đang muốn for all toàn bộ prefab character chẳng hạn,
    xong rồi lấy tất cả AsyncOperation tạo TaskAsync gán cho sequence.
    Chừng nào sequence start thì mới bắt đấu start AsyncOperation.
    Nhưng: để mà lấy được AsyncOperation thì phải start tác vụ Async trước,
    mà khi nào thì thằng AsyncOperation đã chạy rồi.
    //NOTE để sau vậy, giờ thử load all để xong phần spawn, để làm phần AssetBundle trước.
    */

    // #endregion Get SequenceLoading


    public GameObject GetMonster(string prefabName)
    {
        var key = GetMonsterPath(prefabName);
        if (mMonsters.ContainsKey(key))
        {
            return mMonsters[key];
        }

        var monster = AssetLoader.LoadAsset<GameObject>(key) as GameObject;
        if (monster != null){
            mMonsters.Add(key, monster);
        }
        return monster;
    }

    public GameObject GetCharacter(string prefabName)
    {
        var key = GetCharacterPath(prefabName);
        if (mCharacters.ContainsKey(key))
        {
            return mCharacters[key];
        }

        var character = AssetLoader.LoadAsset<GameObject>(key) as GameObject;
        if (character != null){
            mCharacters.Add(key, character);
        }
        return character;
    }

    //NOTE Không nên xài loadAsync một cách tùy tiện, mà phải tùy từng thời điểm, xác định cụ thể trong game.
    //Ví dụ: Load assets cho MainScene khi bắt đầu.
    //Load assets cho BattleScene khi bắt đầu vào ải.
}

public class AssetLoader
{
    #region LoadAssetAsync
        private static IEnumerator _LoaAssetAsync<T>(string path, Action<T> completeCb) where T: UnityEngine.Object
        {
            Debuger.Log("_LoadAsyncGameObject at path: " + path);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localId);
            AssetDatabaseLoadOperation op = AssetDatabase.LoadObjectAsync(path, localId);
            while (!op.isDone)
            {
                yield return null;
            }

            T loadedObject = (T)op.LoadedObject as T;
            if (completeCb != null){
                completeCb(loadedObject);
            }
        }

        public static void LoadAsyncGameObject(string path, Action<GameObject> completeCb)
        {
            LoadAssetAsync<GameObject>(path, completeCb);
        }

        public static void LoadAssetAsync<T>(string path, Action<T> completeCb) where T : UnityEngine.Object
        {
            CoroutineManager.startCoroutine(_LoaAssetAsync(path, completeCb));

            
            //NOTE trong lúc load async nên đẩy game state thành loadingstage, chạy loading UI và disable tất cả thao tác của người chơi.
            //Đồng thời thông báo tên process đang chạy.
            //NOTE tương tự cho UnloadAsync
        }
    #endregion LoadAssetAsync

    #region UnloadAssetAsync

        // public static void UnloadAssetAsync<T>(T asset, Action<T> completeCb) where T : UnityEngine.Object
        // {
        //     CoroutineManager.startCoroutine(_UnLoaAssetAsync(asset, completeCb));
        //     NOTE trong lúc load async nên đẩy game state thành loadingstage, chạy loading UI và disable tất cả thao tác của người chơi.
        //     Đồng thời thông báo tên process đang chạy.
        //     NOTE tương tự cho UnloadAsync
        // }

        // private static IEnumerator _UnLoaAssetAsync<T>(T asset, Action<T> completeCb) where T: UnityEngine.Object
        // {
        //     Debuger.Log("_LoadAsyncGameObject at path: " + path);
        //     GameObject obj = AssetDatabase.
        //     AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localId);
        //     AssetDatabaseLoadOperation op = AssetDatabase.LoadObjectAsync(path, localId);
        //     while (!op.isDone)
        //     {
        //         yield return null;
        //     }

        //     T loadedObject = (T)op.LoadedObject as T;
        //     if (completeCb != null){
        //         completeCb(loadedObject);
        //     }
        // }

    #endregion UnloadAssetAsync
    
    #region Load/UnloadAsset

        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>(path) as T;
        }
    #endregion Load/UnloadAsset
    
}