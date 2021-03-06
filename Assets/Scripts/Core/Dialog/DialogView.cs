﻿using System;
using UnityEngine;
using Util;

namespace Dialog
{
	public class DialogView : MonoBehaviour
	{
        internal DialogManager Manager;
        public string ID { get; internal set; }
		public void Close() => Manager.Close(this.gameObject);
		public virtual void OnOpen() {}
		public virtual void OnClose() {}
	}

    public abstract class DialogView<T> : DialogView
    {
        public virtual void OnOpen(T param) {}
    }
}
