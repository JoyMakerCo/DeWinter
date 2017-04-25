using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class CheckStyleChangeCmd : ICommand<DateTime>
	{
		public void Execute (DateTime day)
		{
			CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
			InventoryModel invModel = DeWinterApp.GetModel<InventoryModel>();

	        // Is it Style Switch Time?
	        // TODO: Commandify style switch
			if (day >= model.NextStyleSwitchDay)
	        {
	        	Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$OLDSTYLE",invModel.CurrentStyle},
					{"$NEWSTYLE",invModel.NextStyle}};
	        	DeWinterApp.OpenMessageDialog(DialogConsts.STYLE_CHANGE_DIALOG, subs);

	            //Actually switching styles
				invModel.CurrentStyle = invModel.NextStyle;
				string nextStyle = invModel.Styles[new Random().Next(1,invModel.Styles.Length)];
				invModel.NextStyle = (nextStyle == invModel.NextStyle ? invModel.Styles[0] : nextStyle);
				model.NextStyleSwitchDay = day.AddDays(new Random().Next(6, 9));
	        }
		}
	}
}

