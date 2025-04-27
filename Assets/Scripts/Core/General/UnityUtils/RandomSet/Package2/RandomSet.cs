using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.UnityUtils
{
	public class RandomSet<T> : ARandomSet<T>
	{
		public enum OutRangeOption
		{
			TakeFirst = 0,
			TakeLast = 1,
			TakeDefault = 2,
		}

		protected string hashID;
		protected string name;
		protected RandomSetData<T> data;
		protected Dictionary<T, float> percent;
		protected OutRangeOption option;

		public string HashID { get => hashID; set => hashID = value; }
		public string Name { get => name; set => name = value; }
		public RandomSetData<T> Data { get => data; set => data = value; }
		public Dictionary<T, float> Percent { get => percent; set => percent = value; }
		public OutRangeOption Option { get => option; set => option = value; }

		public RandomSet(RandomSetData<T> setData, OutRangeOption option = OutRangeOption.TakeDefault)
		{
			this.Data = setData;
			this.Option = option;
			this.Name = setData.Name;
			this.randomize = new System.Random(DateTime.Now.Millisecond);

			this.Percent = new Dictionary<T, float>();

			float percentPlus = 0f;

			foreach (var item in setData.Data)
			{
				this.Percent.Add(item.Key, item.Value + percentPlus);
				//Debug.Log($"Percent: {item.Key} is {item.Value + percentPlus}");
				percentPlus += item.Value;
			}

			if (percentPlus > 1f)
			{
				Debug.LogWarning($"RandomSet: {this.Name} is over 100%");
			}
		}

		public override T Rand()
		{
			var rand = randomize.NextDouble();
			//Debug.Log($"Rand: {rand}");

			foreach (var item in Percent)
			{
				if (item.Value >= rand)
				{
					return item.Key;
				}
			}

			Debugger.Log(LogTags.Module_RandomSet, $"TakeDefalt by rand value: {rand}");
			switch (Option)
			{
				case OutRangeOption.TakeFirst:
					return GetFirstType();
				case OutRangeOption.TakeLast:
					return GetLastType();
				case OutRangeOption.TakeDefault:
					return Data.Default;
			}

			return Data.Default;
		}

		private T GetFirstType()
		{
			return Percent.GetEnumerator().Current.Key;
		}

		private T GetLastType()
		{
			var enumerator = Percent.GetEnumerator();
			T item = enumerator.Current.Key;
			while (enumerator.MoveNext())
			{
				item = enumerator.Current.Key;
			}
			return item;
		}
	}

}
