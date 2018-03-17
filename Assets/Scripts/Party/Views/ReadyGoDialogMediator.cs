using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;
using Core;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView
	{
		public Text dialogText;
		void Start()
		{
			LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			string[] conversationIntroList = model.GetList("conversation_intro");
			dialogText.text = conversationIntroList[Util.RNG.Generate(conversationIntroList.Length)];
			StartCoroutine(CloseDialog());
		}

		IEnumerator CloseDialog()
		{
			yield return new WaitForSeconds(3f);
			Close();
		}

		public override void OnClose ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
		}
	}
}
