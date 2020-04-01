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
            AmbitionApp.GetModel<ParisModel>().Location = null;
            AmbitionApp.GetModel<ParisModel>().Daily = null;
            AmbitionApp.GetModel<InventoryModel>().Market = null;
            AmbitionApp.SendMessage(GameMessages.FADE_OUT, 3f);
        }
    }
}
