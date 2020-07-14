using System;
using System.Collections.Generic;
using Dialog;
using UnityEngine.UI;
using Core;

namespace Ambition
{
	public class RSVPCancelDialogMediator : DialogView<PartyVO>
	{
		public Text BodyText;
		public Text TitleText;

		private PartyVO _party;

		public override void OnOpen(PartyVO p)
		{
			TitleText.text = AmbitionApp.Localize("rsvp_cancel_dialog.title");

			_party = p;

			Dictionary<string, string> subs = new Dictionary<string, string>() {
                {"$PARTYSIZE", AmbitionApp.Localize("party_importance." + ((int)p.Size).ToString())},
				{"$HOSTNAME", p.Host},
				{"$FACTION", AmbitionApp.Localize(p.Faction.ToString())}};

			if (p.Date == AmbitionApp.GetModel<CalendarModel>().Today)
			{
				BodyText.text = AmbitionApp.GetString("rsvp_cancel_dayof_dialog.title", subs);
			}
			else
			{
				BodyText.text = AmbitionApp.GetString("rsvp_cancel_dialog.body", subs);
			}
		}

		public void CancelRSVP() => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, _party);
	}
}
