using UnityEngine;
using System;
using System.Reflection;

public static class ScriptLoader
{

	public static void AddScriptByName(GameObject target, string className)
	{
		// Tìm Type theo tên
		Type type = Type.GetType(className);

		// Nếu không tìm thấy, thử thêm namespace hoặc assembly name
		if (type == null)
		{
			// Gợi ý: nếu script nằm trong namespace, bạn cần ghi đầy đủ
			type = Type.GetType("YourNamespace." + className);

			// Hoặc tìm trong các assembly nếu vẫn chưa được
			if (type == null)
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					type = assembly.GetType(className);
					if (type != null) break;
				}
			}
		}

		// Kiểm tra hợp lệ và gán vào GameObject
		if (type != null && typeof(MonoBehaviour).IsAssignableFrom(type))
		{
			target.AddComponent(type);
			Debug.Log($"Added script: {className}");
		}
		else
		{
			Debug.LogWarning($"Cannot find or use script: {className}");
		}
	}
}
