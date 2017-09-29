using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView
	{
		public Text dialogText;
		void Start()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			string[] conversationIntroList = model.ConversationIntros;
			dialogText.text = conversationIntroList[new System.Random().Next(conversationIntroList.Length)];
			StartCoroutine(CloseDialog());
		}

		IEnumerator CloseDialog()
		{
			yield return new WaitForSeconds(3f);
			AmbitionApp.SendMessage(GameMessages.NEXT_STATE);
			Close();
		}
	}
}
