using System;
using System.Linq;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class CreateInvitationsState : UFlow.UState
	{
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO[] parties = model.GetParties();
            foreach(PartyVO party in parties)
            {
                AmbitionApp.OpenDialog(RSVPDialog.DIALOG_ID, party);
            }
        }
	}
}
