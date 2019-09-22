using System;
using Core;
using Dialog;
using UnityEngine.UI;

namespace Ambition
{
	public class OutOfMoneyDialogMediator : DialogView
	{
		private const string PHRASE_ID = "out_of_money_dialog";

		public Text BodyTxt;
		public Text TitleTxt;
		public Text ButtonLabelTxt;

		public override void OnOpen ()
		{
			base.OnOpen ();
			TitleTxt.text = AmbitionApp.GetString(PHRASE_ID + DialogConsts.TITLE);
			if (AmbitionApp.GetModel<GameModel>().Reputation > 20)
			{
				BodyTxt.text = AmbitionApp.GetString(PHRASE_ID + DialogConsts.BODY);
				ButtonLabelTxt.text = AmbitionApp.GetString(DialogConsts.OK);
			}
			else
			{
				BodyTxt.text = AmbitionApp.GetString("out_of_money_and_rep_dialog" + DialogConsts.BODY);
				ButtonLabelTxt.text = AmbitionApp.GetString("out_of_money_and_rep_dialog" + DialogConsts.OK);
			}
		}

		public override void OnClose ()
		{
			AmbitionApp.Execute<BorrowMoneyCmd>();
			base.OnClose ();
		}
	}
}