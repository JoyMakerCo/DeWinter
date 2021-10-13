using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class MessageViewMediator : DialogView<string>, ISubmitHandler
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
			SetDialog(phrase);
		}

		public void SetDialog(string phrase, Dictionary<string, string> substitutions=null, Action onConfirm=null)
		{
            string txt;
            _onConfirm = onConfirm;
            ConfirmBtn.SetActive(onConfirm != null);

            //string dismissToken = _onConfirm == null ? DialogConsts.
			BodyTxt.text = AmbitionApp.Localize(phrase + DialogConsts.BODY, substitutions);
			TitleTxt.text = AmbitionApp.Localize(phrase + DialogConsts.TITLE, substitutions);

            if (DismissTxt != null)
            {
                txt = AmbitionApp.Localize(phrase + DialogConsts.CANCEL, substitutions);
                DismissTxt.text = !string.IsNullOrEmpty(txt)
                    ? txt
                    : _onConfirm != null
                    ? AmbitionApp.Localize(DialogConsts.DEFAULT_CANCEL)
                    : AmbitionApp.Localize(DialogConsts.DEFAULT_CLOSE);
            }

            if (ConfirmTxt != null)
			{
                txt = AmbitionApp.Localize(phrase + DialogConsts.CONFIRM, substitutions);
                ConfirmTxt.text = string.IsNullOrEmpty(txt)
                    ? AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM)
                    : txt;
			}
        }

        public void Cancel() => Close();
        public void Submit()
        {
            if (_onConfirm == null) Close();
            else Confirm();
        }

		public void Confirm ()
		{
            _onConfirm?.Invoke();
            AmbitionApp.SendMessage(GameMessages.DIALOG_CONFIRM);
            Close();
		}
    }
}
