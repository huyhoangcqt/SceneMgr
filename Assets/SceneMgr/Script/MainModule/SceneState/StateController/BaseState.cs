using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class BaseState : IStateController, IState
	{
		protected Scene Scene;

		public BaseState(Scene scene)
		{
			this.Scene = scene;
		}

		public virtual void Enter() { }
		public virtual void Exit() { }

		protected OperationSequence Sequence;

		protected LoadingSceneController loadingSceneController;

		//Loading scene
		public IEnumerator _IELoadSceneRoutine(string sceneName)
		{
			// Step 1: Load Transition Scene();
			yield return SceneMgr.Instance._IELoadTransitionScene();

			// Step 2: Unload current Scene
			SceneMgr.Instance.UnloadCurrentScene();

			MakeSequenceAsync();

			//Find LoadingSceneController / LoadingUIController
			yield return FindLoadingSceneController();

			//Step 3: Run Seq
			yield return _IERunSequenceAsync();

			//Step 4: Unload Transition Scene();
			SceneMgr.Instance.UnloadTransitionScene();

			yield return _IEDoComplete(sceneName);
		}

		private IEnumerator _IEDoComplete(string sceneName)
		{
			yield return null;
			OnLoadComplete(sceneName);
		}

		public void OnLoadComplete(string sceneName)
		{
			Debug.Log($"On {sceneName} Load Complete!!!");
		}


		/// <summary>
		/// Hàm này sẽ có một lỗi chí mạng khi tìm kiếm quá nhiều MonoBehaviour.
		/// </summary>
		protected IEnumerator FindLoadingSceneController()
		{
			Debuger.Log("[BaseState] FindLoadingSceneController!!");

			int count = 0;
			loadingSceneController = null;

			MonoBehaviour[] behaviours = GameObject.FindObjectsOfType<MonoBehaviour>(true);
			var enumerator = ((IEnumerable)behaviours).GetEnumerator();
			
			while (loadingSceneController == null && enumerator.MoveNext())
			{
				count++;

				var b = enumerator.Current as MonoBehaviour;
				if (b is LoadingSceneController loadingCtrl)
				{
					loadingSceneController = loadingCtrl;
					yield break;
				}

				if (count % 20 == 0)
				{
					yield return null;
				}
			}
		}

		protected void UpdateProgress(float progress, string taskName, float taskProgress)
		{
			if (loadingSceneController != null)
			{
				loadingSceneController.UpdateProgress(progress, taskName, taskProgress);
			}
		}

		protected IEnumerator _IERunSequenceAsync()
		{

			Debuger.Log("[BaseState] _IERunSequenceAsync!!");
			if (Sequence != null)
			{
				Debuger.Log("[BaseState] _IERunSequenceAsync Start !!");

				Debuger.Log($"1. _IERunSequenceAsync > Sequence.IsDone: {Sequence.IsDone}");
				CoroutineManager.Instance.StartCoroutine(Sequence._IEStart());

				Debuger.Log($"2. _IERunSequenceAsync > Sequence.IsDone: {Sequence.IsDone}");
				while (!Sequence.IsDone)
				{
					Debuger.Log($"3. _IERunSequenceAsync > Sequence.IsDone: {Sequence.IsDone}");
					UpdateProgress(Sequence.Progress, Sequence.TaskName, Sequence.TaskProgress);
					yield return null;
				}
				Debuger.Log("[BaseState] Sequence Run Complete!!");
			}
			else
			{
				yield return null;
				Debuger.Log("[BaseState] Sequence Run Complete!!");
			}
		}

		public virtual void MakeSequenceAsync()
		{
			Sequence = new OperationSequence();
			
			// Step 3: Load Target scene
			Sequence.AddTask(SceneMgr.Instance.LoadSceneAsync(Scene.ToString()), $"Load {Scene}");

			Sequence.AddTask(_IEPreLoadAssets(), "Load standard Assets");
			Sequence.AddTask(_IELoadStandardMaterials(), "Load Standard Materials");
			Sequence.AddTask(_IELoadEnvironmentPackage(), "Load Environment Assets");
			Sequence.AddTask(_IEMonsterPackage(), "Load Monsters Package");
			Sequence.AddTask(_IECharacterPackages(), "Load Characters Package");


			Debuger.Log("[BaseState] MakeSequenceAsync Complete!!");
		}


		private IEnumerator _IEPreLoadAssets()
		{
			yield return new WaitForSeconds(1f);
		}
		private IEnumerator _IELoadStandardMaterials()
		{
			yield return new WaitForSeconds(2f);
		}
		private IEnumerator _IELoadEnvironmentPackage()
		{
			yield return new WaitForSeconds(1.2f);
		}
		private IEnumerator _IEMonsterPackage()
		{
			yield return new WaitForSeconds(0.3f);
		}
		private IEnumerator _IECharacterPackages()
		{
			yield return new WaitForSeconds(0.2f);
		}
	}
}
