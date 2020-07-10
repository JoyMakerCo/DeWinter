using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class CreateGossipCmd : ICommand<GossipRewardSpec>
    {
        public void Execute(GossipRewardSpec spec)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            /*
            gossip = null;
 
            if (inventory.Items == null)
            {
                Debug.LogWarning("CreateGossipCmd: 'Items' is null in inventory");
            }
            else
            {
                gossip = Array.Find(inventory.Items, i => i.Type == ItemType.Gossip && i.ID == spec.Faction.ToString());
            }
            if (gossip == null)
            {
                Debug.LogWarningFormat("No gossip item found for faction {0}, making one up", spec.Faction.ToString() );
            }
            */

            ItemVO gossip = new ItemVO();
            gossip.Type = ItemType.Gossip;
            gossip.ID = spec.Faction.ToString();
            bool isPowershift = (spec.Faction == FactionType.Crown || spec.Faction == FactionType.Revolution || 0 == Util.RNG.Generate(0, 2));
            gossip = new ItemVO(gossip);
            gossip.Created = AmbitionApp.GetModel<CalendarModel>().Today;
            gossip.State.Add(ItemConsts.SHIFT, isPowershift ? ItemConsts.POWER : ItemConsts.ALLEGIANCE);
            gossip.Price = (int)spec.Tier; // Price is acting as Tier

            // localize tier, faction, and gossip name template
            var locTier = AmbitionApp.Localize("gossip_tier_" + gossip.Price.ToString());
            var locFaction = AmbitionApp.Localize( spec.Faction.ToString().ToLower() );
            gossip.Name = string.Format( AmbitionApp.Localize( "gossip.name" ), locTier, locFaction );

            inventory.Inventory.Add(gossip);
            inventory.Broadcast();

            //TODO: Make this a factory
        }
    }
}
