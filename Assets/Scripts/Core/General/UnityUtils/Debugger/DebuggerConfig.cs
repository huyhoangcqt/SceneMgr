
using UnityEngine;
using System.Collections.Generic;
using System;


namespace BlackCat.UnityUtils
{
	[CreateAssetMenu(fileName = "DebuggerConfig", menuName = "ScriptableObjects/DebuggerConfig", order = 1)]
	public class DebuggerConfig : ScriptableObject
	{
		public List<LogDataGroup> Configs;

		public LogData GetConfig(string name)
		{
			foreach (var group in Configs)
			{
				if (group.Name == name)
				{
					return group;
				}
				if (group.SubLogs != null)
				{
					foreach (var log in group.SubLogs)
					{

						if (log.Name == name)
						{
							return log;
						}
					}
				}
			}
			return null;
		}
	}

	[Serializable]
	public class LogDataGroup : LogData
	{
		public List<LogData> SubLogs;
	}

	[Serializable]
	public class LogData
	{
		public string Name;
		public Color Color;
		public bool IsShow;
	}
}