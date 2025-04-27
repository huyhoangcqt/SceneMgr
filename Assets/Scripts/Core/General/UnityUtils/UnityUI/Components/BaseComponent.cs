using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowCat.UnityUI
{
	public class BaseComponent : IComponent
	{
		protected Transform _transform;
		protected object _rawData;
		private bool _isSetupDone = false;

		public void Setup(Transform transform, object data)
		{
			if (_isSetupDone) return;
			_isSetupDone = true;

			_transform = transform;
			_rawData = data;

			InitFields();
			ConvertData();
			BuildView();
		}

		public void UpdateData(object newData)
		{
			if (_rawData == newData) return;
			_rawData = newData;

			ConvertData();
			RefreshView();
		}

		public void Show()
		{
			AddListenerEvents();
			OnShow();
		}

		public void Hide()
		{
			RemoveListenerEvents();
			OnHide();
		}

		public void Dispose()
		{
			OnDispose();
		}

		protected virtual void InitFields() { }
		protected virtual void ConvertData() { }
		protected virtual void BuildView() { }
		protected virtual void RefreshView() { }
		protected virtual void AddListenerEvents() { }
		protected virtual void RemoveListenerEvents() { }
		protected virtual void OnShow() { }
		protected virtual void OnHide() { }
		protected virtual void OnDispose() { }
	}
}