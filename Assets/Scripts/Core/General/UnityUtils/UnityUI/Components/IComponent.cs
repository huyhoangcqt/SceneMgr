using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YellowCat.UnityUI
{
	public interface IComponent
	{
		public void Setup(Transform transform, object data);
		public void UpdateData(object newData);
		public void Show();
		public void Hide();
		public void Dispose();
	}
}
