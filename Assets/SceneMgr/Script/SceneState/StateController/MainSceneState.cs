using System;
using System.Collections;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class MainSceneState : BaseState
	{
		public MainSceneState(Scene scene) : base(scene) { }

		public override void Enter()
		{
			Debuger.Log("[MainSceneState] Enter");
			CoroutineManager.Instance.StartCoroutine(_IELoadSceneRoutine(this.Scene.ToString()));
		}

		public override void MakeSequenceAsync()
		{

			Debuger.Log("[MainSceneState] MakeSequenceAsync");
			Sequence = new OperationSequence();

			// Step 3: Load Target scene
			Sequence.AddTask(SceneMgr.Instance.LoadSceneAsync(Scene.ToString()), $"Load {Scene}");

			Sequence.AddTask(_IEPreLoadAssets(), "Main Load standard Assets");
			Sequence.AddTask(_IELoadStandardMaterials(), "Main Load Standard Materials");
			Sequence.AddTask(_IELoadEnvironmentPackage(), "Main Load Environment Assets");
			Sequence.AddTask(_IEMonsterPackage(), "Main Load Monsters Package");
			Sequence.AddTask(_IECharacterPackages(), "Main Load Characters Package");


			Debuger.Log("[MainSceneState] MakeSequenceAsync Complete!!");
		}


		private IEnumerator _IEPreLoadAssets()
		{
			Debuger.Log("[MainSceneState] _IEPreLoadAssets!!");
			yield return 0.1f;
			yield return new WaitForSeconds(1f);
			yield return 0.3f;
			yield return null;
			yield return null;
			yield return null;
			yield return new WaitForSeconds(1f);
			yield return 0.5f;
			yield return null;
			yield return new WaitForSeconds(1f);
			yield return null;
		}
		private IEnumerator _IELoadStandardMaterials()
		{
			Debuger.Log("[MainSceneState] _IELoadStandardMaterials!!");
			yield return new WaitForSeconds(2f);
			yield return 0.3f;
			yield return null;
			yield return null;
			yield return new WaitForSeconds(2f);
			yield return 0.5f;
			yield return null;
			yield return new WaitForSeconds(2f);
			yield return 0.9f;
			yield return null;
		}
		private IEnumerator _IELoadEnvironmentPackage()
		{
			Debuger.Log("[MainSceneState] _IELoadEnvironmentPackage!!");
			yield return new WaitForSeconds(1.2f);
		}
		private IEnumerator _IEMonsterPackage()
		{
			Debuger.Log("[MainSceneState] _IEMonsterPackage!!");
			yield return new WaitForSeconds(0.3f);
		}
		private IEnumerator _IECharacterPackages()
		{
			Debuger.Log("[MainSceneState] _IECharacterPackages!!");
			yield return new WaitForSeconds(0.2f);
		}
	}
}
