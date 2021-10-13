using System;
namespace Ambition
{
    public class UpdateStandingsCmd : Core.ICommand
    {
        public void Execute()
        {
            FactionModel model = AmbitionApp.GetModel<FactionModel>();
            FactionStandingsVO standings;
            model.LastUpdated = AmbitionApp.Calendar.Day;
            model.Standings.Clear();
            foreach(FactionVO faction in model.Factions.Values)
            {
                standings = new FactionStandingsVO();
                standings.Faction = faction.Type;
                standings.Allegiance = faction.Allegiance;
                standings.Power = faction.Power;
                model.Standings.Add(standings);
            }
        }
    }
}
