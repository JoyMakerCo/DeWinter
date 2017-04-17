using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine.UI;
using Core;

namespace DeWinter
{
	public class RSVPCancelDialogMediator : DialogView, IDialog<Party>
	{
		public Text BodyText;
		public Text TitleText;

		private Party _party;

		public void OnOpen(Party p)
		{
			LocalizationModel localization = DeWinterApp.GetModel<LocalizationModel>();
			TitleText.text = localization.GetString("rsvp_cancel_dialog.title");

			_party = p;

			Dictionary<string, string> subs = new Dictionary<string, string>() {
				{"$PARTYSIZE", p.SizeString()},
				{"$HOSTNAME", p.host.Name},
				{"$FACTION", p.faction}};

			if (p.Date == DeWinterApp.GetModel<CalendarModel>().Today)
			{
				BodyText.text = localization.GetString("rsvp_cancel_dayof_dialog.title", subs);
			}
			else
			{
				BodyText.text = localization.GetString("rsvp_cancel_dialog.body", subs);
			}
		}

		public void CancelRSVP()
		{
			_party.RSVP = -1;
			DeWinterApp.SendMessage<Party>(PartyMessages.RSVP, _party);
		}
	}
}