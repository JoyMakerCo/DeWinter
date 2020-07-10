using System;
using System.Linq;
using UnityEngine;
using Core;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
    public class InitPartyState : UState
	{
        public override void OnEnterState()
        {
            Debug.LogFormat("InitPartyState.OnEnterState");
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            IncidentModel incidents = AmbitionApp.GetModel<IncidentModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party);
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO outfit = inventory.GetEquippedItem(ItemType.Outfit);
            AmbitionApp.GetModel<GameModel>().Activity = ActivityType.Party;

            PartyVO party = model.Party = model.Parties.Values.FirstOrDefault(p => p.Date == calendar.Today && p.Attending);

            AmbitionApp.RegisterCommand<PartyRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<PartyRewardsCmd, CommodityVO[]>();

            model.Turn = -1;
            model.Turns = (int)(party?.Size ?? 0);
            if (model.Turns < (party?.RequiredIncidents?.Length ?? 0))
                model.Turns = party?.RequiredIncidents.Length ?? 0;

            if (AmbitionApp.GetModel<FactionModel>()[FactionType.Military].Level >= 3)
            {
                party.MaxIntoxication += 3;
            }

            //model.Drink = 0;
            //model.Intoxication = 0;

            //Damage the Outfit's Novelty, now that the Confidence has already been Tallied
            AmbitionApp.SendMessage(InventoryMessages.DEGRADE_OUTFIT, outfit);

            Debug.LogFormat("Scheduling intro incident: {0}",party.IntroIncident?.ToString());

            incidents.Schedule(party.IntroIncident);
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit);
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, party.Name);
        }
    }
}
