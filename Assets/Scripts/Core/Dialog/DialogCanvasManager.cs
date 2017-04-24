using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

namespace Dialog
{
	[Serializable]
	public struct DialogBinding
	{
		public string Key;
		public GameObject Prefab;
	}

	public class DialogCanvasManager : MonoBehaviour
	{
		public DialogBinding[] DialogPrefabs;

		protected List<GameObject> _dialogs;
		protected Canvas _canvas;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);

			_canvas = this.gameObject.GetComponent<Canvas>();
			_dialogs = new List<GameObject>();
		}

		public GameObject Open(string dialogID)
		{
			DialogBinding binding = Array.Find(DialogPrefabs, d=>d.Key == dialogID);
			if (binding.Equals(default(DialogBinding)) || binding.Prefab == null)
				return null; // Early out

			GameObject dialog = GameObject.Instantiate<GameObject>(binding.Prefab);
			DialogView cmp = dialog.GetComponent<DialogView>();
			if (cmp != null) cmp.Manager = this;
			_dialogs.Add(dialog);
			dialog.transform.SetParent(_canvas.transform, false);
			dialog.GetComponent<RectTransform>().SetAsLastSibling();
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

		public bool Close(GameObject dialog)
		{
			int count = _dialogs.RemoveAll(d => ReferenceEquals(d, dialog));
			if (dialog != null)
			{
				DialogView view = dialog.GetComponent<DialogView>();
				if (view is IDisposable)
					(view as IDisposable).Dispose();
				GameObject.Destroy(dialog);
			}
			return (dialog != null && count > 0);
		}

		public void CloseAll()
		{
			foreach(GameObject d in _dialogs)
			{
				DialogView v = d.GetComponent<DialogView>();
				if (v is IDisposable)
					(v as IDisposable).Dispose();
				GameObject.Destroy(d);
			}
			_dialogs.Clear();
		}

		void OnDestroy()
		{
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}