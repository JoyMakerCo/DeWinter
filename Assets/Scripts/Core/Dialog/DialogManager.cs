using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Core;

namespace Dialog
{
    public class DialogEventArgs : EventArgs
    {
        public string DialogID;
        public GameObject DialogObject;

        public DialogEventArgs(string dialogID, GameObject dialogObject)
        {
            DialogID = dialogID;
            DialogObject = dialogObject;
        }

        public DialogEventArgs(string dialogID) : this(dialogID, null) {}
    }

    public class DialogManager : MonoBehaviour
	{
		public PrefabMapConfig DialogPrefabs;
        public int NumDialogs => _dialogs.Count;
        public event EventHandler<DialogEventArgs> OnOpenDialog;
        public event EventHandler<DialogEventArgs> OnCloseDialog;

        // Store references to dialogs because adding/removing to the display list doesn't happens on frame, not immediately
        protected List<DialogEventArgs> _dialogs; 

		void Awake()
		{
			App.Register<DialogSvc>().RegisterManager(this);
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
            _dialogs = new List<DialogEventArgs>();
            gameObject.SetActive(false);
		}

		public GameObject Open(string dialogID, params string[] args)
		{
			PrefabMap map = Array.Find(DialogPrefabs.Prefabs, p=>p.ID == dialogID);
			if (default(PrefabMap).Equals(map)) return null; //Early out

			GameObject dialog = Instantiate(map.Prefab, this.gameObject.transform);
			if (dialog != null)
			{
				DialogView cmp = dialog.GetComponent<DialogView>();
                DialogEventArgs arg = new DialogEventArgs(dialogID, dialog);
				if (cmp != null)
				{
					cmp.Manager = this;
					cmp.ID = dialogID;
					cmp.OnOpen();
				}
				_dialogs.Add(arg);
				dialog.transform.SetParent(transform, false);
				dialog.GetComponent<RectTransform>().SetAsLastSibling();
                transform.SetAsLastSibling();
                gameObject.SetActive(true);

                OnOpenDialog?.Invoke(this, arg);
            }
            return dialog;
		}

		public GameObject Open<T>(string dialogID, T vo)
		{
			PrefabMap map = Array.Find(DialogPrefabs.Prefabs, p=>p.ID == dialogID);
			if (default(PrefabMap).Equals(map)) return null; //Early out

			GameObject dialog = Instantiate<GameObject>(map.Prefab, this.gameObject.transform);
            if (dialog == null) return null;

			DialogView cmp = dialog.GetComponent<DialogView>();
            DialogEventArgs arg = new DialogEventArgs(dialogID, dialog);
            if (cmp != null)
			{
				cmp.Manager = this;
				cmp.ID = dialogID;
				cmp.OnOpen();
                (cmp as DialogView<T>)?.OnOpen(vo);
            }
            _dialogs.Add(arg);
			dialog.transform.SetParent(transform, false);
			dialog.GetComponent<RectTransform>().SetAsLastSibling();
            transform.SetAsLastSibling();
            gameObject.SetActive(true);

            OnOpenDialog?.Invoke(this, arg);
            return dialog;
		}

        public GameObject Get(string DialogID) => _dialogs.Find(d=>d.DialogID == DialogID)?.DialogObject;

        public bool Close(string dialogID)
		{
            DialogEventArgs arg = _dialogs.First(a => a.DialogID == dialogID);
            if (arg == null) return false;

            arg.DialogObject.GetComponent<DialogView>()?.OnClose();
			_dialogs.Remove(arg);
			Destroy(arg.DialogObject);

            gameObject.SetActive(_dialogs.Count > 0);
            OnCloseDialog?.Invoke(this, new DialogEventArgs(dialogID));
            return true;
		}

		public bool Close(GameObject dialog)
		{
            if (dialog == null) return false;
            DialogEventArgs arg = _dialogs.First(a => a.DialogObject == dialog);
            if (arg == null) return false;
            dialog.GetComponent<DialogView>()?.OnClose();
			Destroy(dialog);
            _dialogs.Remove(arg);
            gameObject.SetActive(_dialogs.Count > 0);
            OnCloseDialog?.Invoke(this, arg);
            dialog = null;
            return true;
		}

		public void CloseAll()
		{
			foreach(DialogEventArgs arg in _dialogs)
			{
                arg.DialogObject?.GetComponent<DialogView>()?.OnClose();
                OnCloseDialog?.Invoke(this, arg);
                if (arg.DialogObject != null) Destroy(arg.DialogObject);
			}
            _dialogs.Clear();
            gameObject.SetActive(false);
		}

        public GameObject[] GetOpenDialogs() => _dialogs.Select(d => d.DialogObject).ToArray();
        public GameObject GetTopDialog() => _dialogs.Count > 0 ? _dialogs[_dialogs.Count-1].DialogObject : null;

        void OnDestroy()
		{
            CloseAll();
			App.Service<DialogSvc>().UnregisterManager(this);
		}
	}
}
