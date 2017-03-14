using System;
using UnityEngine;
using Util;

namespace Dialog
{
	public interface IDialog<T>
	{
		void OnOpen(T vo);
	}

	public class DialogView : Component
	{
		public DialogCanvasManager Manager;

		public void Close()
		{
			if (Manager != null)
				Manager.Close(this.gameObject);
		}
	}
}