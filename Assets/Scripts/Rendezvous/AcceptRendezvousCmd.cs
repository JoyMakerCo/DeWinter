using System;
namespace Ambition
{
    public class AcceptRendezvousCmd : Core.ICommand<RendezVO>
    {
        public void Execute(RendezVO rendez)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            AmbitionApp.GetModel<CharacterModel>().Characters.TryGetValue(rendez.Character, out CharacterVO character);
            PartyVO[] parties = AmbitionApp.Calendar.GetOccasions<PartyVO>(rendez.Day);
            if (Array.Exists(parties, p => p.IsAttending))
            {
                AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, rendez);
            }
            else
            {
                RendezVO[] rendezs = AmbitionApp.Calendar.GetOccasions<RendezVO>(rendez.Day);
                Array.ForEach(parties, p=> AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, p));
                rendez.RSVP = RSVP.Accepted;
                foreach (RendezVO r in rendezs)
                {
                    if (r != rendez)
                        AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, r);
                }
                if (character != null)
                {
                    character.LiaisonDay = rendez.Day;
                    ++character.Favor;
                    model.Broadcast();
                }
                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, rendez);
                AmbitionApp.SendMessage(rendez);
            }
        }
    }
}
