using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowCat.UnityUI
{
	public class BaseWindow : IWindow
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

		protected virtual void InitFields() { } //=> Custom Fields
		protected virtual void ConvertData() { }
		protected virtual void BuildView() { } //=> Setup Components ==> Add to component List
		protected virtual void RefreshView() { } //=> Refresh Components
		protected virtual void AddListenerEvents() { }
		protected virtual void RemoveListenerEvents() { }
		protected virtual void OnShow() { } //=> Show Components
		protected virtual void OnHide() { } //=> Hide Components
		protected virtual void OnDispose() { } //=> Dispose Components
	}
}
