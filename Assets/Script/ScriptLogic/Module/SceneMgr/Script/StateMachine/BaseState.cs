using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YellowCat.SceneMgr
{
	public abstract class BaseState
	{
		public virtual void Enter() { }
		public virtual void Exit() { }
	}
}
