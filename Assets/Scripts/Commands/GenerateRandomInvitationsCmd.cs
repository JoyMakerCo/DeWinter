using System;
namespace Ambition
{
    public class GenerateRandomInvitationsCmd : Core.ICommand
    {
        public void Execute()
        {
            // Create Random parties
            // TODO: Future engagement chance should be a function of faction standing and known associates
            if (Util.RNG.Generate(0, 3) == 0) // Chance of a random future engagement
            {
                DateTime date = AmbitionApp.GetModel<CalendarModel>().Today;
                PartyVO party = new PartyVO
                {
                    Name = "",
                    LocalizationKey = "default",
                    InvitationDate = date,
                    Date = date.AddDays(Util.RNG.Generate(1, 8) + Util.RNG.Generate(1, 8)), // +2d8 days
                    RSVP = RSVP.New,
                    Faction = Util.RNG.TakeRandomExcept(Enum.GetValues(typeof(FactionType)) as FactionType[], FactionType.Neutral)
                };
                AmbitionApp.SendMessage(PartyMessages.INITIALIZE_PARTY, party);
            }
        }
    }
}
