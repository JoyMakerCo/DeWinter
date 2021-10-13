using System;
using System.Collections.Generic;
using Util;
namespace Ambition
{
    public class GenerateRendezvousCmd : Core.ICommand<CalendarModel>
    {
        public void Execute(CalendarModel calendar)
        {
            RendezVO[] rendezs = calendar.GetOccasions<RendezVO>(calendar.Day - 1);
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            CharacterVO notable;
            ChapterVO chapter = AmbitionApp.Game.GetChapter();

            // Let down notables who haven't responded to your invitations
            rendezs = Array.FindAll(rendezs, r => r.RSVP == RSVP.New && r.IsCaller);
            foreach(RendezVO rendez in rendezs)
            {
                notable = characters.GetCharacter(rendez.Character);
                if (notable != null) notable.LiaisonDay = -1;
                rendez.RSVP = RSVP.Declined; // The Command penalizes the player; we're avoiding that here
                AmbitionApp.SendMessage(rendez);
            }

            if ((chapter.TrivialPartyChance + chapter.DecentPartyChance + chapter.GrandPartyChance > 0) && (int)(calendar.Today.DayOfWeek) % 7 == 6) 
            // For whatever reason, DateTime weeks start on Monday
            {
                List<string> locations;
                CalendarEvent[] events;
                bool doSchedule;
                foreach (CharacterVO character in characters.Characters.Values)
                {
                    if (character.IsDateable && character.LiaisonDay < 0 && RNG.Generate(100) < characters.LiaisonChance)
                    {
                        int day = calendar.Day + RNG.Generate(2, 6);
                        events = AmbitionApp.GetEvents(day);
                        doSchedule = (!Array.Exists(events, e => e is RendezVO) && !Array.Exists(events, e => e.IsAttending));
                        if (!doSchedule)
                        {
                            events = AmbitionApp.GetEvents(++day);
                            doSchedule = (!Array.Exists(events, e => e is RendezVO) && !Array.Exists(events, e => e.IsAttending));
                        }
                        if (doSchedule)
                        {
                            locations = new List<string>(character.FavoredLocations);
                            if (locations.Count == 0)
                            {
                                locations = new List<string>(AmbitionApp.Paris.Rendezvous);
                                locations.RemoveAll(l => Array.IndexOf(character.OpposedLocations, l) >= 0);
                            }
                            if (locations.Count > 0) // If there's no viable locations, skip it
                            {
                                RendezVO liaison = new RendezVO()
                                {
                                    Created = -1,
                                    ID = character.ID,
                                    Day = day,
                                    Location = RNG.TakeRandom(locations),
                                    RSVP = RSVP.New,
                                    IsCaller = false
                                };
                                character.LiaisonDay = day;
                                AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, liaison);
                            }
                        }
                    }
                }
            }

        }
    }
}
