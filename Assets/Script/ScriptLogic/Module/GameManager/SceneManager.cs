using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public enum Scene
{
    GameMgrScene,
    MainScene,
    BattleScene,
}

public class SceneMgr : Singleton<SceneMgr>
{
    public Dictionary<Scene, string> mScenes;
    public Scene mCurrent;
    GameObject mLoadingScene;
    
    public SceneMgr() : base()
    {
        Init();
    }

    private void Init()
    {
        mLoadingScene = GameObject.Find("LoadingScene");
        HideLoadingScene();
        mScenes = new Dictionary<Scene, string>();
        mScenes.Add(Scene.GameMgrScene, "GameMgrScene");
        mScenes.Add(Scene.MainScene, "MainScene");
        mScenes.Add(Scene.BattleScene, "BattleScene");
    }

    public void PlayLoadingScene()
    {
        mLoadingScene.SetActive(true);
    }

    public void HideLoadingScene()
    {
        mLoadingScene.SetActive(false);
    }

    public void ChangeSceneAsync(Scene scene, Action onComplete)
    {
        if (mScenes.ContainsKey(scene))
        {
            CoroutineManager.startCoroutine(_ChangeSceneAsync(scene, onComplete));
        }
        else {
            Debuger.Err("The scene: " + scene + " hasn't been registered!");
        }
    }

    IEnumerator _ChangeSceneAsync(Scene newScene, Action onComplete)
    {
        string loadedSceneName = mScenes[newScene];

        string crrSceneName = mScenes[mCurrent];
        Debuger.Log("CurrentSCene: " + crrSceneName);

        PlayLoadingScene();

        //Load new Scene first
        yield return _LoadSceneAsync(loadedSceneName, null);
        mCurrent = newScene;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene.ToString()));

        // then Unload Old scene
        if (crrSceneName != "GameMgrScene"){
            Debuger.Log("2 CurrentSCene: " + crrSceneName);
            yield return _UnloadSceneAsync(crrSceneName, onComplete);
        }
        
        HideLoadingScene();
    }

    IEnumerator _UnloadSceneAsync(string sceneName, Action onComplete)
    {
        UnityEngine.SceneManagement.Scene crrScene = SceneManager.GetSceneByName(sceneName);
        if (crrScene == null){
            Debuger.Err("The scene: " + sceneName + " is not active!");
        }

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        if (onComplete != null){
            onComplete();
        }
    }

    IEnumerator _LoadSceneAsync(string sceneName, Action onComplete, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        
        yield return new WaitForSeconds(2.5f);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (onComplete != null){
            onComplete();
        }
    }
}