using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class RSVPCancelDialogMediator : DialogView, Util.IInitializable<PartyVO>
	{
		public Text BodyText;
		public Text TitleText;

		private PartyVO _party;

		public void Initialize(PartyVO p)
		{
			LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
			TitleText.text = localization.GetString("rsvp_cancel_dialog.title");

			_party = p;

			Dictionary<string, string> subs = new Dictionary<string, string>() {
				{"$PARTYSIZE", p.SizeString()},
				{"$HOSTNAME", p.Host.Name},
				{"$FACTION", p.Faction}};

			if (p.Date == AmbitionApp.GetModel<CalendarModel>().Today)
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
			AmbitionApp.SendMessage<PartyVO>(PartyMessages.RSVP, _party);
		}
	}
}