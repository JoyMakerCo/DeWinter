using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView
	{
		private const float TOTAL_SECONDS = 3f;

		public Text dialogText;
		void Start()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			string[] conversationIntroList = model.ConversationIntros;
			dialogText.text = conversationIntroList[new System.Random().Next(conversationIntroList.Length)];
			StartCoroutine(Countdown());
		}

		IEnumerator Countdown()
		{
			yield return new WaitForSeconds(TOTAL_SECONDS);
			AmbitionApp.SendMessage(PartyMessages.START_ENCOUNTER);
			Close();
		}
	}
}
