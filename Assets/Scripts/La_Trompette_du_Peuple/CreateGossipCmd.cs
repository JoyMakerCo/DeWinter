using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class CreateGossipCmd : ICommand<FactionType>
    {
        public void Execute(FactionType faction)
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            if (inventory.Items == null)
            {
                Debug.LogError("CreateGossipCmd: 'Items' is null in inventory");
                return;
            }
            ItemVO gossip = Array.Find(inventory.Items, i => i.Type == ItemType.Gossip && i.ID == faction.ToString());
            if (gossip == null)
            {
                Debug.LogErrorFormat("No gossip item found for faction {0}", faction.ToString() );
                return;
            }
            bool isPowershift = (faction == FactionType.Crown || faction == FactionType.Revolution || 0 == Util.RNG.Generate(0, 2));
            gossip = new ItemVO(gossip);
            gossip.Created = AmbitionApp.GetModel<CalendarModel>().Today;
            gossip.State.Add(ItemConsts.SHIFT, isPowershift ? ItemConsts.POWER : ItemConsts.ALLEGIANCE);
            gossip.Price = Util.RNG.Generate(3); // Price is acting as Tier
            gossip.Name = AmbitionApp.GetString("gossip_tier_" + gossip.Price.ToString()) + " " + faction.ToString() + "Gossip";

            if (!inventory.Inventory.ContainsKey(gossip.Type))
                inventory.Inventory.Add(gossip.Type, new List<ItemVO>());
            inventory.Inventory[gossip.Type].Add(gossip);
            inventory.Broadcast();

            //TODO: Make this a factory
        }
    }
}
