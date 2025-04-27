

using Unity.VisualScripting;
using UnityEngine;

namespace BlackCat.UnityUtils
{
	public class Debugger : MonoSingletonTemplate<Debugger>
	{
		[SerializeField] DebuggerConfig config;
		protected override void Awake()
		{
			base.Awake();
			if (config == null)
			{
				//config = 
			}
		}

		private void _Log(LogTags tag, string message)
		{
			var cfg = config.GetConfig(tag.ToString());
			if (cfg == null || !cfg.IsShow)
			{
				return;
			}

			Debug.Log($"<color={cfg.Color.ToRGBHex()}>[{tag}]</color> <color=\"black\">-</color> <color=\"white\">{message}</color>");
		}

		private void _Warning(LogTags tag, string message)
		{
			var cfg = config.GetConfig(tag.ToString());
			if (cfg == null || !cfg.IsShow)
			{
				return;
			}

			Debug.LogWarning($"<color={cfg.Color.ToRGBHex()}>[{tag}]</color> <color=\"black\">-</color> <color=\"white\">{message}</color>");
		}

		private void _Error(LogTags tag, string message)
		{
			var cfg = config.GetConfig(tag.ToString());
			if (cfg == null || !cfg.IsShow)
			{
				return;
			}

			Debug.LogError($"<color={cfg.Color.ToRGBHex()}>[{tag}]</color> <color=\"black\">-</color> <color=\"white\">{message}</color>");
		}


		//Public Method
		public static void Log(LogTags tag, string message)
		{
			Instance._Log(tag, message);
		}
		public static void Warning(LogTags tag, string message)
		{
			Instance._Warning(tag, message);
		}
		public static void Error(LogTags tag, string message)
		{
			Instance._Error(tag, message);
		}
	}
}