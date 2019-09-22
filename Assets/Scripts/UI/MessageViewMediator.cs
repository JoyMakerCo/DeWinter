using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class MessageViewMediator : DialogView<string>
	{
		public const string DIALOG_ID = "MESSAGE";
		public Text BodyTxt;
		public Text TitleTxt;
		public Text DismissTxt;
		public Text ConfirmTxt;

		public override void OnOpen(string phrase)
		{
			SetPhrase(phrase);
		}

		public void SetPhrase(string phrase, Dictionary<string, string> substitutions=null)
		{
			string str;
			BodyTxt.text = AmbitionApp.GetString(phrase + DialogConsts.BODY, substitutions);
			TitleTxt.text = AmbitionApp.GetString(phrase + DialogConsts.TITLE, substitutions);

			if (DismissTxt != null)
			{
				str=AmbitionApp.GetString(phrase + DialogConsts.CANCEL, substitutions);
				if (str != null && DismissTxt != null) DismissTxt.text = str;
				else DismissTxt.text = AmbitionApp.GetString(DialogConsts.DEFAULT_CANCEL);
			}
			
			if (ConfirmTxt != null)
			{
				str=AmbitionApp.GetString(phrase + DialogConsts.CONFIRM, substitutions);
				if (str != null && ConfirmTxt != null) ConfirmTxt.text = str;
				else ConfirmTxt.text = AmbitionApp.GetString(DialogConsts.DEFAULT_CONFIRM);
			}
		}

		public override void OnOpen ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_OPENED, ID);
		}

		public override void OnClose ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
		}

		public void Confirm ()
		{
			AmbitionApp.SendMessage(GameMessages.DIALOG_CONFIRM);
			Close();
		}
	}
}
