using System;
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
            PartyVO party = model.UpdateParty();

            IncidentModel incidents = AmbitionApp.GetModel<IncidentModel>();
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO outfit = inventory.GetEquippedItem(ItemType.Outfit);
            AmbitionApp.GetModel<GameModel>().Activity = ActivityType.Party;
            AmbitionApp.RegisterCommand<PartyRewardCmd, CommodityVO>();
            AmbitionApp.RegisterCommand<PartyRewardsCmd, CommodityVO[]>();

            model.Turn = -1;
            model.Turns = (int)(party?.Size ?? 0);
            if (model.Turns < (party?.RequiredIncidents?.Length ?? 0))
                model.Turns = party?.RequiredIncidents.Length ?? 0;

            Debug.LogFormat("Scheduling intro incident: {0}",party.IntroIncident?.ToString());

            incidents.Schedule(party.IntroIncident);
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit);
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, party.Name);
        }
    }
}
