using UFlow;
using System.Collections.Generic;

namespace Ambition
{
    public class CheckMissedPartiesState : UState
    {
        public override void OnEnterState()
        {
            GameModel game = AmbitionApp.GetModel<GameModel>();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO[] parties = model.GetParties((ushort)(game.Day - 1));
            foreach(PartyVO party in parties)
            {
                if (party.RSVP == RSVP.New)
                {
                    game.Reputation -= game.MissedPartyPenalty;
                    Dictionary<string, string> subs = new Dictionary<string, string>() {{"$PARTYNAME",party.ID}};
                    AmbitionApp.OpenDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
                }
            }
        }
    }
}
