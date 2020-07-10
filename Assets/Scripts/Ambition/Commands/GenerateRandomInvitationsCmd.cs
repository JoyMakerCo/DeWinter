using System;
namespace Ambition
{
    public class GenerateRandomInvitationsCmd : Core.ICommand
    {
        public void Execute()
        {
            // Create Random parties
            // TODO: Future engagement chance should be a function of faction standing and known associates
            if ( Util.RNG.Generate(0, 3) == 0) // Chance of a random future engagement
            {
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                GameModel game = AmbitionApp.GetModel<GameModel>();
                ushort day = (ushort)(game.Day + Util.RNG.Generate(1, 8) + Util.RNG.Generate(1, 8));
                int index = model.GetParties(day).Length;
                if (index < 2)
                {
                    PartyVO party = new PartyVO
                    {
                        Name = null,
                        LocalizationKey = "default",
                        InvitationDate = game.Date,
                        Date = game.Date.AddDays(day), // +2d8 days
                        RSVP = RSVP.New,
                        Faction = Util.RNG.TakeRandomExcept(Enum.GetValues(typeof(FactionType)) as FactionType[], FactionType.Neutral)
                    };
                    AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, party);
                }
            }
        }
    }
}
