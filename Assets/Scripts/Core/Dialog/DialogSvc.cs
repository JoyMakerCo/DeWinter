using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Dialog
{
	public class DialogSvc : IAppService
	{
		protected List<DialogCanvasManager> _managers;

		public DialogSvc()
		{
			_managers = new List<DialogCanvasManager>();
		}

		public void Initialize() {}

		public void Dispose()
		{
			CloseAll();
		}

		public void RegisterManager(DialogCanvasManager manager)
		{
			_managers.Add(manager);
		}

		/** Open a dialog by ID on the default canvas. **/
		public GameObject Open(string dialogID)
		{
			GameObject dialog;
			foreach (DialogCanvasManager m in _managers)
			{
				dialog = m.Open(dialogID);
				if (dialog != null)
					return dialog;
			}
			return null;
		}

		/** Open a dialog by ID on the default canvas. **/
		public GameObject Open<T>(string dialogID, T data)
		{
			GameObject dialog;
			foreach (DialogCanvasManager m in _managers)
			{
				dialog = m.Open<T>(dialogID, data);
				if (dialog != null)
					return dialog;
			}
			return null;
		}

		/** Close the named Dialog on every canvas. **/
		public void Close (string dialogID)
		{
			foreach (DialogCanvasManager m in _managers)
			{
				if (m.Close(dialogID))
					return;
			}
		}

		/** Close the given dialog object. **/
		public void Close (GameObject dialog)
		{
			foreach (DialogCanvasManager m in _managers)
			{
				if (m.Close(dialog))
					return;
			}
		}

		/** Close All dialogs on all Canvases. **/
		public void CloseAll()
		{
			foreach (DialogCanvasManager m in _managers)
			{
				m.CloseAll();
			}
		}
	}
}