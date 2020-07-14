using System;
using System.Linq;
using System.Collections.Generic;
namespace Ambition
{
    public class AdvanceDayCmd : Core.ICommand
    {
        public void Execute()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            GameModel game = AmbitionApp.GetModel<GameModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();

            AmbitionApp.GetModel<IncidentModel>().SetDay(++calendar.Day);

            if (paris.Location?.ID == ParisConsts.HOME)
            {
                if (game.Exhaustion > 0) game.Exhaustion.Value = 0;
                else game.Exhaustion.Value = -1;
            }
            else if (paris.Location != null)
            {
                game.Exhaustion.Value++;
            }
            paris.Location = null;
            AmbitionApp.SendMessage(InventoryMessages.UNEQUIP, ItemType.Outfit);
            AmbitionApp.GetModel<ParisModel>().Daily = null;
            AmbitionApp.GetModel<InventoryModel>().Market = null;
        }
    }
}
