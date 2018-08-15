using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Core;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
	{
		public PrefabMapConfig DialogPrefabs;

		protected Dictionary<GameObject, string> _dialogs;

		void Awake()
		{
			App.Service<DialogSvc>().RegisterManager(this);
            CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
            if (group == null) group = gameObject.AddComponent<CanvasGroup>();
            Image image = gameObject.GetComponent<Image>();
            if (image == null) image = gameObject.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
            this.GetComponent<RectTransform>().anchorMin = Vector2.zero;
            this.GetComponent<RectTransform>().anchorMax = Vector2.one;
            group.blocksRaycasts = true;
            group.interactable = true;
            group.ignoreParentGroups = true;
   			_dialogs = new Dictionary<GameObject, string>();
            gameObject.SetActive(false);
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
				dialog.transform.SetParent(transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
                transform.SetAsLastSibling();
                gameObject.SetActive(true);
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
				dialog.transform.SetParent(transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
                transform.SetAsLastSibling();
                gameObject.SetActive(true);
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

            gameObject.SetActive(_dialogs.Count > 0);
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

                gameObject.SetActive(_dialogs.Count > 0);
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
				Destroy(dialog.Key);
			}
            gameObject.SetActive(false);
			_dialogs.Clear();
		}

		void OnDestroy()
		{
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}
