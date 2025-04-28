using System;
using System.Collections;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class BattleSceneState : BaseState
	{
		public BattleSceneState(Scene scene) : base(scene) { }

		public override void Enter()
		{
			CoroutineManager.Instance.StartCoroutine(_IELoadSceneRoutine(this.Scene.ToString()));
		}

		public override void MakeSequenceAsync()
		{
			Sequence = new OperationSequence();

			// Step 3: Load Target scene
			Sequence.AddTask(SceneMgr.Instance.LoadSceneAsync(Scene.ToString()), $"Load {Scene}");

			Sequence.AddTask(_IEPreLoadAssets(), "Battle Load standard Assets");
			Sequence.AddTask(_IELoadStandardMaterials(), "Battle Load Standard Materials");
			Sequence.AddTask(_IELoadEnvironmentPackage(), "Battle Load Environment Assets");
			Sequence.AddTask(_IEMonsterPackage(), "Battle Load Monsters Package");
			Sequence.AddTask(_IECharacterPackages(), "Battle Load Characters Package");
		}


		private IEnumerator _IEPreLoadAssets()
		{
			yield return new WaitForSeconds(1f);
			yield return new WaitForSeconds(1f);
			yield return new WaitForSeconds(1f);
		}
		private IEnumerator _IELoadStandardMaterials()
		{
			yield return new WaitForSeconds(2f);
			yield return new WaitForSeconds(2f);
			yield return new WaitForSeconds(2f);
			yield return new WaitForSeconds(2f);
		}
		private IEnumerator _IELoadEnvironmentPackage()
		{
			yield return new WaitForSeconds(1.2f);
			yield return new WaitForSeconds(1.2f);
			yield return new WaitForSeconds(1.2f);
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
