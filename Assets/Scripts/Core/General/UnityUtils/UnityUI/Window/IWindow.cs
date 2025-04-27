using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace YellowCat.UnityUI
{
	/// <summary>
	/// 
	/// Setup      => lần đầu (gán transform, data, build view gốc)
	/// UpdateData => đổi data(update nội dung view)
	/// Show       => gắn listener
	/// Hide       => gỡ listener
	/// Dispose    => dọn sạch
	/// </summary>

	public interface IWindow
	{
		public void Setup(Transform transform, object data);
		public void UpdateData(object newData);
		public void Show();
		public void Hide();
		public void Dispose();
	}
}
