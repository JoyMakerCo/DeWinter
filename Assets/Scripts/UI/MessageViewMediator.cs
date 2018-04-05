using System;
using Dialog;
using UnityEngine.UI;

namespace Ambition
{
	public class MessageViewMediator : DialogView, Util.IInitializable<MessageDialogVO>
	{
		public const string DIALOG_ID = "MESSAGE";
		public Text BodyTxt;
		public Text TitleTxt;
		public Text ButtonLabelTxt;

		public void Initialize(MessageDialogVO vo)
		{
			BodyTxt.text = vo.Body;
			TitleTxt.text = vo.Title;
			ButtonLabelTxt.text = vo.Button;
		}

		public override void OnOpen ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_OPENED, ID);
		}

		public override void OnClose ()
		{
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
		}
	}
}