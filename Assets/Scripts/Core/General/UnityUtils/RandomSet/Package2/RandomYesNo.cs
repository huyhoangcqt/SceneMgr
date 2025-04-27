using System;

namespace BlackCat.UnityUtils
{
	public class RandomYesNo : ARandom<bool>
	{
		protected string name;
		protected float percent;

		public RandomYesNo(string name)
		{
			this.name = name;
			this.percent = 0.5f;
			this.randomize = new System.Random(DateTime.Now.Millisecond);
		}

		public RandomYesNo(string name, float yesPercent)
		{
			this.name = name;
			this.percent = yesPercent;
			this.randomize = new System.Random(DateTime.Now.Millisecond);
		}

		public override bool Rand()
		{
			var rand = randomize.NextDouble();
			if (rand > percent)
			{
				return false;
			}
			return true;
		}
	}
}