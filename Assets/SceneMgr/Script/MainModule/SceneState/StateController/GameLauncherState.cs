using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCat.SceneMgr
{
	public class GameLauncherState : BaseState
	{
		public GameLauncherState(Scene scene) : base(scene) { }
		public override void Enter()
		{
			//Step 1: Load loading Scene

			//Step 2: Create Sequence
			// - Load Scene Async
			// - Load Resource

			//Step 3: Get LoadingSceneController
			// - Run sequence, onComplete ( active scene, UnLoad LoadingScene)
		}

		public override void Exit()
		{

		}
	}
}
