using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public class InitPlayerCmd : Core.ICommand<string>
    {
        public void Execute(string playerID)
        {
            PlayerConfig config = Resources.Load<PlayerConfig>(Filepath.PLAYERS + playerID);
            if (config != null)
            {
                InventoryModel inventory = AmbitionApp.Inventory;
                AmbitionApp.Execute<RestorePlayerCmd, PlayerConfig>(config);
                AmbitionApp.Game.Livre = config.Livre;
                foreach (ItemConfig def in config.Inventory)
                {
                    inventory.Inventory.Add(inventory.Instantiate(def.name));
                }
            }
        }
    }
}
