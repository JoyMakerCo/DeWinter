using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public class OpenDialogEventArgs : EventArgs
    {
        public readonly string DialogID;
        public readonly GameObject DialogObject;
        public readonly DialogView ViewComponent;

        public OpenDialogEventArgs(string dialogID) : this(dialogID, null) {}
        public OpenDialogEventArgs(string dialogID, GameObject dialogObject)
        {
            DialogID = dialogID;
            DialogObject = dialogObject;
            ViewComponent = DialogObject.GetComponent<DialogView>();
        }
    }

    public class CloseDialogEventArgs : EventArgs
    {
        public readonly string DialogID;
        public CloseDialogEventArgs(string dialogID) => DialogID = dialogID;
    }

    public class DialogManager : MonoBehaviour
	{
		public PrefabMap[] DialogPrefabs;
        public int NumDialogs => _dialogs.Count;
        public event EventHandler<OpenDialogEventArgs> OnOpenDialog;
        public event EventHandler<CloseDialogEventArgs> OnCloseDialog;
        public CanvasGroup Blocker = null;

        protected List<OpenDialogEventArgs> _dialogs = new List<OpenDialogEventArgs>();

		void Start() => gameObject.SetActive(false);

        public GameObject Open(string dialogID, params string[] args)
        {
            GameObject prefab = Array.Find(DialogPrefabs, p => p.DialogID == dialogID).DialogPrefab;
            if (prefab == null) return null;

            GameObject dialog = Instantiate(prefab, this.gameObject.transform, false);
            OpenDialogEventArgs arg = new OpenDialogEventArgs(dialogID, dialog);
            if (arg.ViewComponent != null)
            {
                arg.ViewComponent.Manager = this;
                arg.ViewComponent.ID = dialogID;
            }
            _dialogs.Add(arg);
            Blocker?.transform.SetSiblingIndex(transform.childCount - 2);
            gameObject.SetActive(true);
            arg.ViewComponent?.OnOpen();
            OnOpenDialog?.Invoke(this, arg);
            return dialog;
		}

        public GameObject Open<T>(string dialogID, T vo)
        {
            GameObject prefab = Array.Find(DialogPrefabs, p => p.DialogID == dialogID).DialogPrefab;
            if (prefab == null) return null;

            GameObject dialog = Instantiate(prefab, this.gameObject.transform);
            OpenDialogEventArgs arg = new OpenDialogEventArgs(dialogID, dialog);
            if (arg.ViewComponent != null)
            {
                arg.ViewComponent.Manager = this;
                arg.ViewComponent.ID = dialogID;
                arg.ViewComponent.OnOpen();
                (arg.ViewComponent as DialogView<T>)?.OnOpen(vo);
            }
            _dialogs.Add(arg);
            dialog.transform.SetParent(transform, false);
            gameObject.SetActive(true);

            OnOpenDialog?.Invoke(this, arg);
            Blocker?.transform.SetSiblingIndex(transform.childCount - 2);
            return dialog;
		}

        public GameObject Get(string DialogID) => _dialogs.Find(d=>d.DialogID == DialogID)?.DialogObject;

        public bool Close(string dialogID)
		{
            OpenDialogEventArgs arg = _dialogs.FirstOrDefault(a => a.DialogID == dialogID);
            if (arg == null || !_dialogs.Remove(arg)) return false;

            bool openDialogs = _dialogs.Count > 0;
            arg.ViewComponent?.OnClose();
            (arg.ViewComponent as IDisposable)?.Dispose();
			Destroy(arg.DialogObject);
            gameObject.SetActive(openDialogs);
            if (openDialogs) Blocker?.transform.SetSiblingIndex(_dialogs.Count - 1);
            OnCloseDialog?.Invoke(this, new CloseDialogEventArgs(dialogID));
            return true;
		}

		public bool Close(GameObject dialog)
		{
            if (dialog == null) return false;
            OpenDialogEventArgs arg = _dialogs.FirstOrDefault(a => a.DialogObject == dialog);
            if (arg == null || !_dialogs.Remove(arg)) return false;

            bool openDialogs = _dialogs.Count > 0;
            arg.ViewComponent?.OnClose();
            (arg.ViewComponent as IDisposable)?.Dispose();
            Destroy(dialog);
            gameObject.SetActive(openDialogs);
            dialog = null;
            if (openDialogs) Blocker?.transform.SetSiblingIndex(_dialogs.Count - 1);
            OnCloseDialog?.Invoke(this, new CloseDialogEventArgs(arg.DialogID));
            return true;
		}

		public void CloseAll()
		{
            CloseDialogEventArgs[] ids = new CloseDialogEventArgs[_dialogs.Count];
            int i = 0;
			foreach(OpenDialogEventArgs arg in _dialogs)
			{
                arg.ViewComponent?.OnClose();
                (arg.ViewComponent as IDisposable)?.Dispose();
                if (arg.DialogObject != null) Destroy(arg.DialogObject);
                ids[i++] = new CloseDialogEventArgs(arg.DialogID);
			}
            _dialogs.Clear();
            if (OnCloseDialog != null)
            {
                Array.ForEach(ids, id => OnCloseDialog(this, id));
            }
            gameObject.SetActive(false);
		}

        public GameObject[] GetOpenDialogs() => _dialogs.Select(d => d.DialogObject).ToArray();
        public GameObject GetTopDialog() => _dialogs.Count > 0 ? _dialogs[_dialogs.Count-1].DialogObject : null;

        void OnDestroy()
		{
            foreach (Transform child in this.gameObject.transform)
            {
                Destroy(child.gameObject);
            }
            _dialogs.ForEach(a => (a.ViewComponent as IDisposable)?.Dispose());
            _dialogs.Clear();
		}

        [Serializable]
        public struct PrefabMap
        {
            public string DialogID;
            public GameObject DialogPrefab;
        }
	}
}
