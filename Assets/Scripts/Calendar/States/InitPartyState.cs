using System;
using UnityEngine;
using Core;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class InitPartyState : UState, Core.IState
    {
        public override void OnEnter()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel story = AmbitionApp.Story;
            PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
            IncidentVO incident = story.GetIncident(party?.IntroIncident);
            LocalizationModel loc = AmbitionApp.GetModel<LocalizationModel>();
            AmbitionApp.Game.Activity = ActivityType.Party;

            model.Turn = -1;
            model.Incidents = null;
            if (party == null) model.Turns = 0;
            else
            {
                string[] names = Enum.GetNames(typeof(PartySize));
                int index = Array.IndexOf(names, party.Size.ToString());
                if (index < 0) index = 0;
                if (index < model.NumTurnsByPartySize.Length)
                    model.Turns = model.NumTurnsByPartySize[index];
                else
                    model.Turns = model.NumTurnsByPartySize[model.NumTurnsByPartySize.Length - 1];
                if (party.RequiredIncidents?.Length > model.Turns)
                    model.Turns = party.RequiredIncidents.Length;
            }

            if (incident == null)
            {
                IncidentVO[] incidents = AmbitionApp.Story.GetIncidents(IncidentType.PartyIntro);
                IncidentVO[] candidates = Array.FindAll(incidents, i => Array.IndexOf(i.Factions, party.Faction) >= 0);
                if (candidates.Length == 0)
                {
                    candidates = Array.FindAll(incidents, i => ((i.Factions?.Length ?? 0) == 0 || (i.Factions.Length == 1 && i.Factions[0] == FactionType.None)));
                }
                incident = Util.RNG.TakeRandom(candidates);
            }

            loc.SetPartyOutfit(AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO);
            loc.SetParty(party);

            AmbitionApp.SendMessage(CalendarMessages.SCHEDULE, incident);
        }
    }
}
