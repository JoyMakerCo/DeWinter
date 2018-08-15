using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Dialog
{
	public class DialogSvc : IAppService
	{
		protected List<DialogManager> _managers;

		public DialogSvc()
		{
			_managers = new List<DialogManager>();
		}

		public void Initialize() {}

		public void Dispose()
		{
			CloseAll();
		}

		public void RegisterManager(DialogManager manager)
		{
			_managers.Add(manager);
		}

		public void UnregisterManager(DialogManager manager)
		{
			_managers.Remove(manager);
		}

		/** Open a dialog by ID on the default canvas. **/
		public GameObject Open(string dialogID)
		{
			GameObject dialog = null;
			GameObject currDialog;
			foreach (DialogManager m in _managers)
			{
				currDialog = m.Open(dialogID);
				if (dialog == null) dialog = currDialog;
			}
			return dialog;
		}

		/** Open a dialog by ID **/
// TODO: Apply to Panel objects, not Canvas objects
		public GameObject Open<T>(string dialogID, T data)
		{
			GameObject dialog = null;
			GameObject currDialog;
			foreach (DialogManager m in _managers)
			{
				currDialog = m.Open<T>(dialogID, data);
				if (dialog == null) dialog = currDialog;
			}
			return dialog;
		}

		public void Close (string dialogID)
		{
			foreach (DialogManager m in _managers)
			{
				if (m.Close(dialogID))
					return;
			}
		}

		/** Close the given dialog object. **/
		public void Close (GameObject dialog)
		{
			foreach (DialogManager m in _managers)
			{
				if (m.Close(dialog))
					return;
			}
		}

		/** Close All dialogs on all Canvases. **/
		public void CloseAll()
		{
			foreach (DialogManager m in _managers)
			{
				m.CloseAll();
			}
		}
	}
}