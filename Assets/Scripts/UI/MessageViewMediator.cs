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
        public GameObject ConfirmBtn;

        private Action _onConfirm = null;

        public override void OnOpen(string phrase)
		{
			SetPhrase(phrase);
		}

		public void SetPhrase(string phrase, Dictionary<string, string> substitutions=null)
		{
			BodyTxt.text = AmbitionApp.GetString(phrase + DialogConsts.BODY, substitutions);
			TitleTxt.text = AmbitionApp.GetString(phrase + DialogConsts.TITLE, substitutions);

			if (DismissTxt != null)
			{
                DismissTxt.text = AmbitionApp.GetString(phrase + DialogConsts.CANCEL, substitutions)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CANCEL);
			}
			
			if (ConfirmTxt != null)
			{
                ConfirmTxt.text = AmbitionApp.GetString(phrase + DialogConsts.CONFIRM, substitutions)
                    ?? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
			}
		}

        public void SetConfirmAction(Action onConfirm)
        {
            _onConfirm = onConfirm;
            ConfirmBtn.SetActive(onConfirm != null);
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
            _onConfirm?.Invoke();
            AmbitionApp.SendMessage(GameMessages.DIALOG_CONFIRM);
		}
	}
}
