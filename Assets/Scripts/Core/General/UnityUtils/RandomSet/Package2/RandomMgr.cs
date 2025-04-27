
using System;
using System.Collections.Generic;

namespace BlackCat.UnityUtils
{
	public enum RandomSetType
	{
		String = 0,
		Enum = 1,
	}

	public class RandomMgr
	{

		/// <summary>
		/// RANDOM SET
		/// </summary>

		public class RandomSetMetaData
		{
			public string Name;
			public RandomSetType Type;

			public RandomSetMetaData(string name, RandomSetType type)
			{
				this.Name = name;
				this.Type = type;
			}
		}

		private static Dictionary<string, RandomSetString> randomSetStr = new Dictionary<string, RandomSetString>();
		private static Dictionary<string, RandomSetEnum> randomSetEnum = new Dictionary<string, RandomSetEnum>();

		private static Dictionary<string, RandomSetMetaData> randomMetaData = new Dictionary<string, RandomSetMetaData>();


		//PUBLIC METHOD
		public static void CreateSet(string name, Dictionary<string, float> percent)
		{
			var data = new RandomSetData<string>(name, percent);
			var randomSet = new RandomSetString(data);

			if (!randomMetaData.ContainsKey(name))
			{
				randomMetaData.Add(name, new RandomSetMetaData(name, RandomSetType.String));
				randomSetStr.Add(name, randomSet);
			}
			else
			{
				Debugger.Warning(LogTags.Module_RandomSet, $"The RandomSet with name: {name} is already Existed!");
			}
		}

		public static void CreateSet(string name, Dictionary<Enum, float> percent)
		{
			var data = new RandomSetData<Enum>(name, percent);
			var randomSet = new RandomSetEnum(data);

			if (!randomMetaData.ContainsKey(name))
			{
				randomMetaData.Add(name, new RandomSetMetaData(name, RandomSetType.Enum));
				randomSetEnum.Add(name, randomSet);
			}
			else
			{
				Debugger.Warning(LogTags.Module_RandomSet, $"The RandomSet with name: {name} is already Existed!");
			}
		}

		public static RandomSet<T> GetSet<T>(string name)
		{
			if (!randomMetaData.ContainsKey(name))
			{
				return null;
			}
			var metaData = randomMetaData[name];
			switch (metaData.Type)
			{
				case RandomSetType.String:
					return GetRandomSetString(name) as RandomSet<T>;
				case RandomSetType.Enum:
					return GetRandomSetEnum(name) as RandomSet<T>;
			}
			return null;
		}

		public static RandomSetString GetRandomSetString(string name)
		{
			if (!randomSetStr.ContainsKey(name))
			{
				return null;
			}
			return randomSetStr[name];
		}


		public static RandomSetEnum GetRandomSetEnum(string name)
		{
			if (!randomSetEnum.ContainsKey(name))
			{
				return null;
			}
			return randomSetEnum[name];
		}


		//RandomSet but create independence
		public static RandomSet<T> CreateSet<T>(string name, Dictionary<T, float> percent) where T : Enum
		{
			var data = new RandomSetData<T>(name, percent);
			var randomSet = new RandomSet<T>(data);
			return randomSet;
		}


		/// <summary>
		/// OTHER RANDOM (like RandomYesNo)
		/// </summary>
		private static Dictionary<string, RandomYesNo> randomYesNo = new Dictionary<string, RandomYesNo>();

		//PUBLIC METHOD

		public static void Create(string name)
		{
			Create(name, 0.5f);
		}

		public static void Create(string name, float percent)
		{
			var randomize = new RandomYesNo(name, percent);

			if (!randomYesNo.ContainsKey(name))
			{
				randomYesNo.Add(name, randomize);
			}
			else
			{
				Debugger.Warning(LogTags.Module_RandomSet, $"The RandomSet with name: {name} is already Existed!");
			}
		}

		public static RandomYesNo Get(string name)
		{
			if (!randomYesNo.ContainsKey(name))
			{
				return null;
			}

			return randomYesNo[name];
		}

	}
}