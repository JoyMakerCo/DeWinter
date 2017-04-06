using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

namespace Dialog
{
	public class DialogCanvasManager : MonoBehaviour
	{
		public GameObject[] DialogPrefabs;

		protected Dictionary<string, GameObject> _prefabs;
		protected Dictionary<string, GameObject> _dialogs;
		protected Canvas _canvas;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);

			_canvas = this.gameObject.GetComponent<Canvas>();
			_prefabs = new Dictionary<string, GameObject>();
			_dialogs = new Dictionary<string, GameObject>();

			foreach (GameObject prefab in DialogPrefabs)
			{
				_prefabs.Add(prefab.name, prefab);
			}
		}

		public GameObject Open(string dialogID)
		{
			Close(dialogID);
			GameObject prefab;
			if (!_prefabs.TryGetValue(dialogID, out prefab))
				return null;
			
			GameObject dialog = GameObject.Instantiate<GameObject>(prefab);
			DialogView cmp = dialog.GetComponent<DialogView>();
			if (cmp != null) cmp.Manager = this;
			_dialogs.Add(dialogID, dialog);
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

		public bool Close(string dialogID)
		{
			GameObject dlg;
			if (_dialogs.TryGetValue(dialogID, out dlg) && dlg != null)
			{
				GameObject.Destroy(dlg);
			}
			return _dialogs.Remove(dialogID);
		}

		public bool Close(GameObject dialog)
		{
			if (dialog == null) return false;
			IEnumerable<string> keys = _dialogs.Keys.Where(x => _dialogs[x] == dialog);
			bool result = keys.Count() > 0;
			foreach(string key in keys) _dialogs.Remove(key);
			GameObject.Destroy(dialog);
			return result;
		}

		public void CloseAll()
		{
			foreach(KeyValuePair<string, GameObject> kvp in _dialogs)
			{
				GameObject.Destroy(kvp.Value);
			}
			_dialogs.Clear();
		}

		void OnDestroy()
		{
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}