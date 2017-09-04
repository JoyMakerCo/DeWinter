using System;
using UnityEngine;
using Util;

namespace Dialog
{
	public class DialogView : MonoBehaviour
	{
		public string ID;

		[HideInInspector]
		public DialogCanvasManager Manager;

		public void Close()
		{
			OnClose();
			Manager.Close(this.gameObject);
		}

		public virtual void OnClose()
		{
			Destroy(this);
			if (this is IDisposable)
				((IDisposable)this).Dispose();
		}
	}
}
