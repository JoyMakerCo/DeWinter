using System;
using Dialog;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class LocalizedDialogView : DialogView
	{
		public string LocalizationKey;
		public Text TitleText;
		public Text BodyText;
//		public Text ButtonLabel;

		public override void OnOpen()
		{
			base.OnOpen();
			if (!string.IsNullOrEmpty(LocalizationKey))
			{
				LocalizationModel lmod = AmbitionApp.GetModel<LocalizationModel>();
				if (TitleText != null)
					TitleText.text = lmod.GetString(LocalizationKey + DialogConsts.TITLE);
				if (BodyText != null)
					BodyText.text = lmod.GetString(LocalizationKey + DialogConsts.BODY);
//				if (ButtonLabel != null)
//					ButtonLabel.text = lmod.GetString(LocalizationKey + DialogConsts.CONFIRM);
			}
		}
	}
}

