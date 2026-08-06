using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCat.SceneMgr
{
	public interface IState
	{
		public void Enter();
		public void Exit();
	}
}
