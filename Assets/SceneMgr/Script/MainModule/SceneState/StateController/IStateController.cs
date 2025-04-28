using System;
using System.Collections;

namespace YellowCat.SceneMgr
{
	public interface IStateController
	{
		public IEnumerator _IELoadSceneRoutine(string sceneName);

		public void MakeSequenceAsync();

		public void OnLoadComplete();
	}
}
