using UFlow;
using System;
using System.Collections.Generic;

namespace Ambition
{
    public class CheckMissedPartiesState : UState, Core.IState
    {
        public override void OnEnter()
        {
            CalendarModel cal = AmbitionApp.Calendar;
            PartyVO[] parties = cal.GetOccasions<PartyVO>(cal.Day - 1);
            PartyVO party = Array.Find(parties, p => p.RSVP == RSVP.New);
            if (party != null)
            {
                CommodityVO penalty = new CommodityVO(CommodityType.Credibility, AmbitionApp.GetModel<PartyModel>().IgnoreInvitationPenalty);
                Dictionary<string, string> subs = new Dictionary<string, string>();
                LocalizationModel loc = AmbitionApp.GetModel<LocalizationModel>();
                party.RSVP = RSVP.Declined;
                subs["$PARTYNAME"] = loc.GetPartyName(party);
                subs["$CREDIBILITY"] = penalty.Value.ToString();
                AmbitionApp.OpenDialog(DialogConsts.MISSED_RSVP_DIALOG, subs);
                AmbitionApp.SendMessage(penalty);
            }
            else
            {
                RendezVO[] dates = cal.GetOccasions<RendezVO>(cal.Day - 1);
                RendezVO date = Array.Find(dates, d => d.RSVP == RSVP.New);
                if (date != null)
                {
                    CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
                    CharacterVO character = characters.GetCharacter(date.Character);
                    bool isPenalized = !date.IsCaller;
                    date.RSVP = RSVP.Declined;
                    if (character != null) character.LiaisonDay = -1;
                    AmbitionApp.SendMessage(date);
                    if (isPenalized)
                    {
                        int penalty = characters.MissedRendezvousPenalty;
                        CommodityVO favorPenalty = new CommodityVO()
                        {
                            Type = CommodityType.Favor,
                            ID = date.Character,
                            Value = penalty
                        };
                        AmbitionApp.SendMessage(favorPenalty);
                        Dictionary<string, string> subs = new Dictionary<string, string>();
                        subs["$SHORTNAME"] = AmbitionApp.GetModel<LocalizationModel>().GetShortName(character, date.Character);
                        subs["$FAVOR"] = penalty.ToString();
                        AmbitionApp.OpenDialog(DialogConsts.MISSED_RENDEZVOUS_DIALOG, subs);
                    }
                }
            }
        }
    }
}
