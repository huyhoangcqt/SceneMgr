using UnityEngine;

namespace YellowCat.SceneMgr
{
	public class Debuger
	{
		private static bool isDebugMode = false;

		public static void Log(string message)
		{
			if (isDebugMode)
			{
				Debug.Log(message);
			}
		}

		public static void Wrn(string message)
		{
			if (isDebugMode)
			{
				Debug.LogWarning(message);
			}
		}

		public static void Err(string message)
		{
			if (isDebugMode)
			{
				Debug.LogError(message);
			}
		}
	}
}