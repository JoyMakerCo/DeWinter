using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialog;
using Core;

namespace Ambition
{
	public class ReadyGoDialogMediator : DialogView, IPointerClickHandler
	{
		public const string DIALOG_ID = "READY_GO";
		public Text dialogText;
		public Text PromptText;
		void Start()
		{
			LocalizationModel model = AmbitionApp.GetModel<LocalizationModel>();
			string[] conversationIntroList = model.GetList("conversation_intro");
			dialogText.text = conversationIntroList[Util.RNG.Generate(conversationIntroList.Length)];
			PromptText.enabled = true;
		}

		IEnumerator Countdown()
		{
			dialogText.text = "3...";
			yield return new WaitForSeconds(1f);
			dialogText.text = "2...";
			yield return new WaitForSeconds(1f);
			dialogText.text = "1...";
			yield return new WaitForSeconds(1f);
			Close();
		}

		public void OnPointerClick(PointerEventData eventData)
	    {
			PromptText.enabled = false;
			StartCoroutine(Countdown());
        }

		public override void OnClose ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
		}
	}
}
