
using System.Collections.Generic;


namespace BlackCat.UnityUtils
{
	public abstract class ARandomSet<T> : ARandom<T>
	{
		public Dictionary<T, float> chance = new Dictionary<T, float>();
	}
}