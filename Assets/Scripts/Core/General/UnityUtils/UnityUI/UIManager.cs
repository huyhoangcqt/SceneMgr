using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowCat.UnityUI
{
    public class UIManager : Singleton<UIManager>
    {
		private const string WindowPrefab = "Prefabs/UI/Window";

        Dictionary<string, BaseWindow> _windows = new Dictionary<string, BaseWindow>();

        public T OpenWindow<T>(string windowName, Transform parent, object data = null) where T : BaseWindow, new()
        {
            if (_windows.TryGetValue(windowName, out var window))
            {
                window.Show();
                return window as T;
            }

			//Create Instantiate Window
			T newWindow = new T();
			Transform windowTransform = InstantiateWindow(windowName, parent);
			newWindow.Setup(windowTransform, data);
			newWindow.Show();

			_windows.Add(windowName, newWindow);
			return newWindow;
		}

		private Transform InstantiateWindow(string windowName, Transform parent)
		{
			// TODO: Load prefab từ Resources hoặc AssetBundle
			//GameObject prefab = MgrMgr.Resource.LoadWindow(windowName) as GameObject;
			var path = $"{WindowPrefab}/{windowName}";
			GameObject prefab = Resources.Load<GameObject>(path);
			GameObject instance = GameObject.Instantiate(prefab, parent);
			return instance.transform;
		}

		public void CloseWindow(string  windowName)
        {
            if (_windows.TryGetValue(windowName, out var window))
            {
                window.Hide();
                window.Dispose();
                _windows.Remove(windowName);
            }
        }

		public void ShowWindow(string windowName)
		{
			if (_windows.TryGetValue(windowName, out var window))
			{
				window.Show();
			}
		}

		public void HideWindow(string windowName)
		{
			if (_windows.TryGetValue(windowName, out var window))
			{
				window.Hide();
			}
		}
	}
}
