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
                {"$PARTYSIZE", AmbitionApp.GetString("party_importance." + ((int)p.Importance).ToString())},
				{"$HOSTNAME", p.Host},
				{"$FACTION", AmbitionApp.GetString(p.Faction)}};

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
            _party.RSVP = RSVP.Declined;
			AmbitionApp.SendMessage<PartyVO>(_party);
		}
	}
}
