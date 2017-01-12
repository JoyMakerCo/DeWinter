using System;
using UnityEngine;

namespace Dialog
{
	public interface IDialog<T>
	{
		void Initialize(T vo);
	}

	public class DialogView : MonoBehaviour
	{
		protected DialogCanvasManager _mgr;

		void Start()
		{
			_mgr = GetComponentInParent<DialogCanvasManager>();
		}

		public void Close()
		{
			if (_mgr != null)
				_mgr.Close(this.gameObject);
		}

		public virtual void OnClose() {}
	}
}