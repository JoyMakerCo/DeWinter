﻿using System;
using System.Collections.Generic;
using UFlow;

namespace Ambition
{
	public class StyleChangeState : UState
	{
		public override void OnEnterState ()
		{
			CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();

			if (calendar.Today >= calendar.NextStyleSwitchDay)
	        {
				InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
				string nextStyle = inventory.NextStyle;

				if (nextStyle == null)
				{
					nextStyle = inventory.Styles[new Random().Next(1,inventory.Styles.Length)];
					if (nextStyle == inventory.CurrentStyle) nextStyle = inventory.Styles[0];
				}

				Dictionary<string, string> subs = new Dictionary<string, string>(){
					{"$OLDSTYLE",inventory.CurrentStyle},
					{"$NEWSTYLE",nextStyle}};
// TODO: Uncomment this when there's a better way to present it
// AmbitionApp.OpenMessageDialog("style_change_dialog", subs);

	            //Actually switching styles
				inventory.CurrentStyle = nextStyle;
				nextStyle = inventory.Styles[new Random().Next(1,inventory.Styles.Length)];
				inventory.NextStyle = (nextStyle == inventory.NextStyle ? inventory.Styles[0] : nextStyle);
				calendar.NextStyleSwitchDay = calendar.Today.AddDays(new Random().Next(6, 9));
	        }			
		}
	}
}

