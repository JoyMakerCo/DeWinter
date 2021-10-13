using Dialog;
using Core;
using UnityEngine;

namespace Ambition
{
	public class DialogSvc : MonoBehaviour, IAppService
	{
		public DialogManager dialogManager;
		public FMODEvent OpenDialogSnd;
		public FMODEvent CloseDialogSnd;

		/** Open a dialog by ID on the default canvas. **/
		public GameObject Open(string dialogID) => dialogManager.Open(dialogID);

		/** Open a dialog by ID **/
		public GameObject Open<T>(string dialogID, T data) => dialogManager.Open(dialogID, data);

		public void Close(string dialogID) => dialogManager.Close(dialogID);

		/** Close the given dialog object. **/
		public void Close(GameObject dialog) => dialogManager.Close(dialog);

		/** Close All dialogs on all Canvases. **/
		public void CloseAll() => dialogManager.CloseAll();

		public void Dispose() => dialogManager.CloseAll();

		private void Awake()
        {
			App.Register(this);
			dialogManager.OnOpenDialog += HandleDialogOpen;
			dialogManager.OnCloseDialog += HandleDialogClosed;
        }

		private void HandleDialogOpen(object sender, OpenDialogEventArgs args)
        {
			AmbitionApp.SendMessage(GameMessages.DIALOG_OPENED, args.DialogID);
			AmbitionApp.SendMessage(AudioMessages.PLAY, OpenDialogSnd);
        }

		private void HandleDialogClosed(object sender, CloseDialogEventArgs args)
		{
			AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, args.DialogID);
			AmbitionApp.SendMessage(AudioMessages.PLAY, CloseDialogSnd);
		}
	}
}
