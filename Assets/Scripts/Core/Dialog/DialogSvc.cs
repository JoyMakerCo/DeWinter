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

		public void UnregisterManager(DialogCanvasManager manager)
		{
			_managers.Remove(manager);
		}

		/** Open a dialog by ID on the default canvas. **/
		public GameObject Open(string dialogID)
		{
			GameObject dialog = null;
			GameObject currDialog;
			foreach (DialogCanvasManager m in _managers)
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
			foreach (DialogCanvasManager m in _managers)
			{
				currDialog = m.Open<T>(dialogID, data);
				if (dialog == null) dialog = currDialog;
			}
			return dialog;
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