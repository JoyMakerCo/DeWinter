using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Core;

namespace Dialog
{
	public class DialogCanvasManager : MonoBehaviour
	{
		public PrefabMapConfig DialogPrefabs;

		protected Dictionary<GameObject, string> _dialogs;
		protected Canvas _canvas;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);

			_canvas = this.gameObject.GetComponent<Canvas>();
			_dialogs = new Dictionary<GameObject, string>();
		}

		public GameObject Open(string dialogID)
		{
			PrefabMap map = Array.Find(DialogPrefabs.Prefabs, p=>p.ID == dialogID);
			if (default(PrefabMap).Equals(map)) return null; //Early out

			GameObject dialog = Instantiate<GameObject>(map.Prefab, this.gameObject.transform);
			if (dialog != null)
			{
				DialogView cmp = dialog.GetComponent<DialogView>();
				if (cmp != null) cmp.Manager = this;
				_dialogs.Add(dialog, dialogID);
				dialog.transform.SetParent(_canvas.transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
			}
			return dialog;
		}

		public GameObject Open<T>(string dialogID, T vo)
		{
			GameObject dlg = Open(dialogID);
			if (dlg != null)
			{
				DialogView cmp = dlg.GetComponent<DialogView>();
				if (cmp is IDialog<T>)
					(cmp as IDialog<T>).OnOpen(vo);
			}
			return dlg;
		}

		public bool Close(string dialogID)
		{
			KeyValuePair<GameObject, string> dialog = _dialogs.FirstOrDefault(d => d.Value == dialogID);
			if (dialog.Equals(default(KeyValuePair<GameObject,string>))) return false;

			DialogView view = dialog.Key.GetComponent<DialogView>();
			if (view is IDisposable)
				(view as IDisposable).Dispose();

			_dialogs.Remove(dialog.Key);
			GameObject.Destroy(dialog.Key);
			return true;
		}

		public bool Close(GameObject dialog)
		{
			bool closed = _dialogs.Remove(dialog);
			if (dialog != null)
			{
				DialogView view = dialog.GetComponent<DialogView>();
				if (view is IDisposable)
					(view as IDisposable).Dispose();
				GameObject.Destroy(dialog);
				closed = true;
			}
			dialog = null;
			return closed;
		}

		public void CloseAll()
		{
			DialogView cmp;
			foreach(KeyValuePair<GameObject, string> dialog in _dialogs)
			{
				cmp = dialog.Key.GetComponent<DialogView>();
				if (cmp is IDisposable)
					(cmp as IDisposable).Dispose();
				GameObject.Destroy(dialog.Key);
			}
			_dialogs.Clear();
		}

		void OnDestroy()
		{
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}