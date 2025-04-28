using System;
using System.Collections;
using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class BattleSceneState : BaseState
	{
		public BattleSceneState(Scene scene) : base(scene) { }
		private bool isFirstTime = false;

		public override void Enter()
		{
			CoroutineManager.Instance.StartCoroutine(_IELoadSceneRoutine(this.Scene.ToString()));
		}

		public override void MakeSequenceAsync()
		{
			Sequence = new OperationSequence();

			// Step 3: Load Target scene
			Sequence.AddTask(SceneMgr.Instance.LoadSceneAsync(Scene.ToString()), $"Load {Scene}");

			if (!isFirstTime)
			{
				isFirstTime = true;
				Sequence.AddTask(_IEPreLoadAssets(), "Battle Load standard Assets");
				Sequence.AddTask(_IELoadStandardMaterials(), "Battle Load Standard Materials");
				Sequence.AddTask(_IELoadEnvironmentPackage(), "Battle Load Environment Assets");
				Sequence.AddTask(_IEMonsterPackage(), "Battle Load Monsters Package");
				Sequence.AddTask(_IECharacterPackages(), "Battle Load Characters Package");
			}
			else
			{
				Sequence.AddTask(_IELoadEnvironmentPackage2(), "Battle Load Environment Assets");
				Sequence.AddTask(_IEMonsterPackage2(), "Battle Load Monsters Package");
				Sequence.AddTask(_IECharacterPackages2(), "Battle Load Characters Package");
				Sequence.AddTask(_IECharacterPackages3(), "Battle Load Characters 3");
				Sequence.AddTask(_IECharacterPackages4(), "Battle Load Characters 4");
				Sequence.AddTask(_IECharacterPackages5(), "Battle Load Characters 5");
				Sequence.AddTask(_IECharacterPackages6(), "Battle Load Characters 6");
			}
		}


		private IEnumerator _IEPreLoadAssets()
		{
			yield return new WaitForSeconds(1f);
			yield return 0.3f;
			yield return new WaitForSeconds(1f);
			yield return 0.6f;

			yield return new WaitForSeconds(0.1f);
			yield return 0.6f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.7f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.8f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.92f;
		}
		private IEnumerator _IELoadStandardMaterials()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.2f);
			yield return 0.3f;
			yield return new WaitForSeconds(0.2f);
			yield return 0.5f;

			yield return new WaitForSeconds(0.1f);
			yield return 0.6f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.7f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.8f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.92f;
		}
		private IEnumerator _IELoadEnvironmentPackage()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.2f);
			yield return 0.3f;
			yield return new WaitForSeconds(0.2f);
			yield return 0.5f;

			yield return new WaitForSeconds(0.1f);
			yield return 0.6f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.7f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.8f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.92f;
		}
		private IEnumerator _IEMonsterPackage()
		{
			yield return new WaitForSeconds(0.3f);
		}
		private IEnumerator _IECharacterPackages()
		{
			yield return new WaitForSeconds(0.2f);
		}

		//Other times

		private IEnumerator _IELoadEnvironmentPackage2()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.2f);
			yield return 0.5f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}
		private IEnumerator _IEMonsterPackage2()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}
		private IEnumerator _IECharacterPackages2()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}

		private IEnumerator _IECharacterPackages3()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}

		private IEnumerator _IECharacterPackages4()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}

		private IEnumerator _IECharacterPackages5()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}

		private IEnumerator _IECharacterPackages6()
		{
			yield return new WaitForSeconds(0.1f);
			yield return 0.2f;
			yield return new WaitForSeconds(0.1f);
			yield return 0.9f;
		}
	}
}
