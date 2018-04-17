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
				if (cmp != null)
				{
					cmp.Manager = this;
					cmp.ID = dialogID;
					cmp.OnOpen();
				}
				_dialogs.Add(dialog, dialogID);
				dialog.transform.SetParent(_canvas.transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
			}
			return dialog;
		}

		public GameObject Open<T>(string dialogID, T vo)
		{
			PrefabMap map = Array.Find(DialogPrefabs.Prefabs, p=>p.ID == dialogID);
			if (default(PrefabMap).Equals(map)) return null; //Early out

			GameObject dialog = Instantiate<GameObject>(map.Prefab, this.gameObject.transform);
			if (dialog != null)
			{
				DialogView cmp = dialog.GetComponent<DialogView>();
				if (cmp != null)
				{
					cmp.Manager = this;
					cmp.ID = dialogID;
					if (cmp is IInitializable<T>)
						(cmp as IInitializable<T>).Initialize(vo);
					cmp.OnOpen();
				}
				_dialogs.Add(dialog, dialogID);
				dialog.transform.SetParent(_canvas.transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
			}
			return dialog;
		}

		public bool Close(string dialogID)
		{
			KeyValuePair<GameObject, string> dialog = _dialogs.FirstOrDefault(d => d.Value == dialogID);
			if (dialog.Equals(default(KeyValuePair<GameObject,string>))) return false;

			DialogView view = dialog.Key.GetComponent<DialogView>();
			if (view != null) view.OnClose();

			_dialogs.Remove(dialog.Key);
			GameObject.Destroy(dialog.Key);
			return true;
		}

		public bool Close(GameObject dialog)
		{
			bool closed = (dialog != null);
			if (closed)
			{
				_dialogs.Remove(dialog);
				DialogView view = dialog.GetComponent<DialogView>();
				view.OnClose();
				GameObject.Destroy(dialog);
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
				if (cmp != null) cmp.OnClose();
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