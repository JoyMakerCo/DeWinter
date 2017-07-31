using System;
using Dialog;
using UnityEngine.UI;

namespace Ambition
{
	public class MessageViewMediator : DialogView, IDialog<MessageDialogVO>
	{
		public Text BodyTxt;
		public Text TitleTxt;
		public Text ButtonLabelTxt;

		public void OnOpen(MessageDialogVO vo)
		{
			BodyTxt.text = vo.Body;
			TitleTxt.text = vo.Title;
			ButtonLabelTxt.text = vo.Button;
		}
	}
}