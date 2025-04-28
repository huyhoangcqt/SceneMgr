using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace YellowCat.SceneMgr
{

	public enum Scene
	{
		GameLauncher,
		LoadingScene,
		MainScene,
		BattleScene,
	}


	public class SceneMgr : MonoSingletonTemplate<SceneMgr>
	{
		public StateMachine<Scene> _stateMachine;

		public AsyncOperation CurrentLoadingOperation { get; private set; }
		private string crrSceneName = Scene.GameLauncher.ToString();

		protected override void OnAwake()
		{
			DontDestroyOnLoad(this);
			registerStates();
		}

		private void registerStates()
		{
			var states = new Dictionary<Scene, IState>()
			{
				{Scene.GameLauncher, new GameLauncherState(Scene.GameLauncher) },
				{Scene.MainScene, new MainSceneState(Scene.MainScene) },
				{Scene.BattleScene, new BattleSceneState(Scene.BattleScene) },

			};
			_stateMachine = new StateMachine<Scene>(states, Scene.GameLauncher);
		}

		public void LoadScene(Scene scene)
		{
			_stateMachine.ChangeState(scene);
		}

		//	// Step 1: Load Transition Scene();
		//	yield return _IELoadTransitionScene();

		//	// Step 2: Unload current Scene
		//	_IEUnloadCurrentScene();

		//	// Step 3: Load Target scene
		//	yield return _IELoadSceneAsync(sceneName);

		//	// Step 4: Running Oepration Sequence

		//	// Step 5: Unload Transition Scene

		public AsyncOperation LoadSceneAsync(string sceneName)
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				Debug.Log($"[SceneMgr] _IELoadSceneAsync {sceneName}");
				crrSceneName = sceneName;
				return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			}
			return null;
		}


		public IEnumerator _IELoadSceneAsync(string sceneName)
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				Debug.Log($"[SceneMgr] _IELoadSceneAsync {sceneName}");
				CurrentLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				crrSceneName = sceneName;
			}
			yield return new WaitUntil( () => { return CurrentLoadingOperation.isDone;  });
		}

		public IEnumerator _IELoadTransitionScene()
		{
			if (!SceneManager.GetSceneByName(Scene.LoadingScene.ToString()).isLoaded)
			{
				yield return SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString(), LoadSceneMode.Additive);
			}
		}

		public void UnloadTransitionScene()
		{
			UnloadScene(Scene.LoadingScene.ToString());
		}


		public void UnloadScene(string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				SceneManager.UnloadSceneAsync(sceneName);
			}
		}


		public void UnloadCurrentScene()
		{
			if (crrSceneName != null && crrSceneName != Scene.GameLauncher.ToString())
			{
				UnloadScene(crrSceneName);
			}
		}
	}
}