using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class MessageViewMediator : DialogView, Util.IInitializable<string>
	{
		public const string DIALOG_ID = "MESSAGE";
		public Text BodyTxt;
		public Text TitleTxt;
		public Text DismissTxt;
		public Text ConfirmTxt;

		public void Initialize(string phrase)
		{
			SetPhrase(phrase);
		}

		public void SetPhrase(string phrase, Dictionary<string, string> substitutions=null)
		{
			string str;
			Core.LocalizationModel model = AmbitionApp.GetModel<Core.LocalizationModel>();
			BodyTxt.text = model.GetString(phrase + DialogConsts.BODY, substitutions);
			TitleTxt.text = model.GetString(phrase + DialogConsts.TITLE, substitutions);

			str=model.GetString(phrase + DialogConsts.CANCEL, substitutions);
			if (str != null && DismissTxt != null) DismissTxt.text = str;
			
			str=model.GetString(phrase + DialogConsts.CONFIRM, substitutions);
			if (str != null && ConfirmTxt != null) ConfirmTxt.text = str;
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
