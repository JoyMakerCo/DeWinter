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
		protected List<GameObject> _dialogs;
		protected Canvas _canvas;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);

			_canvas = this.gameObject.GetComponent<Canvas>();
			_prefabs = new Dictionary<string, GameObject>();
			_dialogs = new List<GameObject>();

			foreach (GameObject prefab in DialogPrefabs)
			{
				_prefabs.Add(prefab.name, prefab);
			}
		}

		public GameObject Open(string dialogID)
		{
			GameObject prefab;
			if (!_prefabs.TryGetValue(dialogID, out prefab))
				return null;
			
			GameObject dialog = GameObject.Instantiate<GameObject>(prefab);
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
			if (dialog != null) GameObject.Destroy(dialog);
			return (dialog != null && count > 0);
		}

		public void CloseAll()
		{
			_dialogs.ForEach(GameObject.Destroy);
			_dialogs.Clear();
		}

		void OnDestroy()
		{
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}