using System;
using System.Linq;
using System.Collections.Generic;
namespace Ambition
{
    public class AdvanceDayCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.GetModel<CalendarModel>().Day++;
            GameModel game = AmbitionApp.GetModel<GameModel>();
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();

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
            AmbitionApp.GetModel<ParisModel>().Daily = null;
            AmbitionApp.GetModel<InventoryModel>().Market = null;
            AmbitionApp.SendMessage(GameMessages.FADE_OUT, 3f);
        }
    }
}
