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
		private string targetSceneName = Scene.GameLauncher.ToString();

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

			//switch (scene)
			//{
			//	case Scene.GameLauncher:
			//		break;
			//	case Scene.MainScene:
			//		//Run Step 1, 2, 3
					
			//		//Make Operation (Load newScene, LoadResource coroutine)
			//		//Call to Start
			//		//Send updateProgress Event each progress update
					
			//		//Call OnLoadingComplete
			//		break;
			//	case Scene.BattleScene:
			//		break;
			//}
		}


		//public IEnumerator LoadSceneRoutine(string sceneName)
		//{
		//	// Step 1: Load Transition Scene();
		//	yield return _IELoadTransitionScene();

		//	// Step 2: Unload current Scene
		//	_IEUnloadCurrentScene();

		//	// Step 3: Load Target scene
		//	yield return _IELoadSceneAsync(sceneName);

		//	// Step 4: Running Oepration Sequence

		//}


		/// <summary>
		/// Callback from Loading Scene State
		/// </summary>
		//public void OnLoadingComplete()
		//{
		//	// Step 5: Unload Transition Scene
		//	_IEUnloadTransitionScene();
		//}

		public AsyncOperation LoadSceneAsync(string sceneName)
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				Debug.Log($"[SceneMgr] _IELoadSceneAsync {sceneName}");
				targetSceneName = sceneName;
				crrSceneName = sceneName;
				return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			}
			return null;
		}


		public IEnumerator _IELoadSceneAsync(string sceneName/*, bool isActive*/)
		{
			if (!SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				Debug.Log($"[SceneMgr] _IELoadSceneAsync {sceneName}");
				targetSceneName = sceneName;
				CurrentLoadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				//CurrentLoadingOperation.allowSceneActivation = isActive;
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

		private IEnumerator _IEUnloadScene(string sceneName)
		{
			if (SceneManager.GetSceneByName(sceneName).isLoaded)
			{
				yield return SceneManager.UnloadSceneAsync(sceneName);
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