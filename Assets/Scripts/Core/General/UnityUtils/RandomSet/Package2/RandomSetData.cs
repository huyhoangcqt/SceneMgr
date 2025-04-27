using System.Collections.Generic;

namespace BlackCat.UnityUtils
{
	public class RandomSetData<T>
	{
		private Dictionary<T, float> data;
		public T Default;
		public string Name;

		public Dictionary<T, float> Data { get => data; set => data = value; }

		public RandomSetData(string Name, Dictionary<T, float> data, T defaultValue)
		{
			this.Name = Name;
			this.Data = new Dictionary<T, float>();
			this.Default = defaultValue;

			foreach (var item in data)
			{
				this.Data.Add(item.Key, MathExtension.Floor(item.Value, 2));
			}
		}

		public RandomSetData(string Name, Dictionary<T, float> data)
		{
			this.Name = Name;
			this.Data = new Dictionary<T, float>();
			this.Default = data.Keys.GetEnumerator().Current;

			foreach (var item in data)
			{
				this.Data.Add(item.Key, MathExtension.Floor(item.Value, 2));
			}
		}
	}
}