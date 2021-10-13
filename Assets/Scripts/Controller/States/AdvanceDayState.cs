using System;
using System.Collections.Generic;
using UFlow;
namespace Ambition
{
    public class AdvanceDayState : UState, Core.IState
    {
        public override void OnEnter()
        {
            AmbitionApp.Calendar.Day++;
            AmbitionApp.Story.Update(true);
            AmbitionApp.Paris.Location = null;
            AmbitionApp.Paris.Daily = null;
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit);
            AmbitionApp.GetModel<InventoryModel>().Market = null;
            AmbitionApp.Calendar.Broadcast();
        }
    }
}
