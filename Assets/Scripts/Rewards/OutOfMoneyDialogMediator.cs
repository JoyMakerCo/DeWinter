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

		private GameModel _gameModel = AmbitionApp.GetModel<GameModel>();
		private LocalizationModel _local = AmbitionApp.GetModel<LocalizationModel>();

		public override void OnOpen ()
		{
			base.OnOpen ();
			TitleTxt.text = _local.GetString(PHRASE_ID + DialogConsts.TITLE);
			if (_gameModel.Reputation > 20)
			{
				BodyTxt.text = _local.GetString(PHRASE_ID + DialogConsts.BODY);
				ButtonLabelTxt.text = _local.GetString(DialogConsts.OK);
			}
			else
			{
				BodyTxt.text = _local.GetString("out_of_money_and_rep_dialog" + DialogConsts.BODY);
				ButtonLabelTxt.text = _local.GetString("out_of_money_and_rep_dialog" + DialogConsts.OK);
			}
		}

		public override void OnClose ()
		{
			AmbitionApp.Execute<BorrowMoneyCmd>();
			base.OnClose ();
		}
	}
}