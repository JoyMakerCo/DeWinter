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
            PartyModel model = AmbitionApp.RegisterModel<PartyModel>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyVO party = Array.Find(calendar.GetEvents<PartyVO>(), p => p.Attending);
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];
            ItemVO outfit = inventory.GetEquippedItem(ItemType.Outfit);

            model.Party = party;
            model.Turn = 0;
            model.Turns = (int)(party?.Size ?? 0);
            if (model.Turns < (party?.RequiredIncidents?.Length ?? 0))
                model.Turns = party?.RequiredIncidents.Length ?? 0;

            if (AmbitionApp.GetModel<FactionModel>()[FactionType.Military].Level >= 3)
            {
                party.MaxIntoxication += 3;
            }

            model.Drink = 0;
            model.Intoxication = 0;

            //Damage the Outfit's Novelty, now that the Confidence has already been Tallied
            AmbitionApp.SendMessage(InventoryMessages.DEGRADE_OUTFIT, outfit);

            Debug.LogFormat("Scheduling intro incident: {0}",party.IntroIncident?.ToString());
            calendar.Schedule(party.IntroIncident, calendar.Today);

            MapModel mapModel = AmbitionApp.GetModel<MapModel>();
            if (!mapModel.LoadMap(party))
            {
                MapVO[] maps = Array.FindAll(mapModel.Maps,
                    m => m.Faction == model.Party.Faction || m.Faction == FactionType.Neutral
                );

                // TODO: Figure out significance of tagging scheme
                //MapModel.MapConfig[] tagConfigs = Array.FindAll(configs, c=>(
                //party.Tags == null || party.Tags.Length == 0
                //Array.TrueForAll(party.Tags, t => Array.IndexOf(c.Tags, t) >= 0)));
                //party.MapID = Util.RNG.TakeRandom(tagConfigs.Length == 0 ? configs : tagConfigs).Name;
                mapModel.Map.Value = Util.RNG.TakeRandom(maps);
            }
        }
    }
}
