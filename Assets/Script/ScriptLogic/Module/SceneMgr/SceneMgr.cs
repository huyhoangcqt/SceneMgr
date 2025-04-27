using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum SceneState
{
	Loading,
	Transition,
	Main,
	Battle
}

public class SceneName
{
	public const string Loading = "LoadingScene";
	public const string GameMgrScene = "GameMgrScene";
	public const string Main = "MainScene";
	public const string Battle = "BattleScene";
}

namespace YellowCat.SceneMgr
{
	public class SceneMgr : MonoBehaviour
	{
		public static SceneMgr Instance { get; private set; }

		public AsyncOperation CurrentLoadingOperation { get; private set; }
		private string crrSceneName = SceneName.GameMgrScene;
		private string targetSceneName = SceneName.GameMgrScene;

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
			DontDestroyOnLoad(this);
		}

		public void LoadScene(string sceneName)
		{
			StartCoroutine(LoadSceneRoutine(sceneName));
		}

		private IEnumerator LoadSceneRoutine(string sceneName)
		{
			//Step1: Load loading Scene
			//Step2: UnLoad Current scene
			//Step3: Load Target scene
			//Step4: Unload loading Scene


			// Step 1: Load LoadingScene nếu chưa có
			if (!SceneManager.GetSceneByName("LoadingScene").isLoaded)
			{
				yield return SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
			}

			// Step 2: Unload current Scene
			if (crrSceneName != null && crrSceneName != SceneName.GameMgrScene)
			{
				UnloadScene(crrSceneName);
			}

			// Step 3: Load Target scene
			targetSceneName = sceneName;
			Debug.Log("Load Scene");
			CurrentLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			CurrentLoadingOperation.allowSceneActivation = false;
			crrSceneName = sceneName;
		}

		public void ActivateLoadedScene()
		{
			if (CurrentLoadingOperation != null)
			{
				CurrentLoadingOperation.allowSceneActivation = true;
			}
		}

		public void UnloadScene(string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				SceneManager.UnloadSceneAsync(sceneName);
			}
		}
	}
}