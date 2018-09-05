using System;
using UnityEngine;
using Util;

namespace Dialog
{
	public class DialogView : MonoBehaviour
	{
		public string ID
		{
			get;
			internal set;
		}

		internal DialogManager Manager;

		public void Close()
		{
			Manager.Close(this.gameObject);
		}

		public virtual void OnOpen() {}
		public virtual void OnClose() {}
	}
}
