using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class StyleChangeState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
			InventoryModel invModel = AmbitionApp.GetModel<InventoryModel>();

	        // Is it Style Switch Time?
	        // TODO: Commandify style switch
			if (calendar.Today >= calendar.NextStyleSwitchDay)
	        {
	        	Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$OLDSTYLE",invModel.CurrentStyle},
					{"$NEWSTYLE",invModel.NextStyle}};
				AmbitionApp.OpenMessageDialog("style_change_dialog", subs);

	            //Actually switching styles
				invModel.CurrentStyle = invModel.NextStyle;
				string nextStyle = invModel.Styles[new Random().Next(1,invModel.Styles.Length)];
				invModel.NextStyle = (nextStyle == invModel.NextStyle ? invModel.Styles[0] : nextStyle);
				calendar.NextStyleSwitchDay = calendar.Today.AddDays(new Random().Next(6, 9));
	        }			
		}
	}
}

