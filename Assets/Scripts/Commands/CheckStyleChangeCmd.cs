using System;
using System.Collections.Generic;
using Core;

namespace Ambition
{
	public class CheckStyleChangeCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
			InventoryModel invModel = AmbitionApp.GetModel<InventoryModel>();

	        // Is it Style Switch Time?
	        // TODO: Commandify style switch
			if (day >= model.NextStyleSwitchDay)
	        {
	        	Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$OLDSTYLE",invModel.CurrentStyle},
					{"$NEWSTYLE",invModel.NextStyle}};
				AmbitionApp.OpenMessageDialog("style_change_dialog", subs);

	            //Actually switching styles
				invModel.CurrentStyle = invModel.NextStyle;
				string nextStyle = invModel.Styles[new Random().Next(1,invModel.Styles.Length)];
				invModel.NextStyle = (nextStyle == invModel.NextStyle ? invModel.Styles[0] : nextStyle);
				model.NextStyleSwitchDay = day.AddDays(new Random().Next(6, 9));
	        }
		}
	}
}

