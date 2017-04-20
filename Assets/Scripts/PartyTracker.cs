using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DeWinter
{
	public class PartyTracker : MonoBehaviour
	{
		private const int MAX_PARTY_RANGE = 15;
	    private Text myText;
	    public Party nextParty;

	    void Start()
	    {
	        myText = GetComponent<Text>();
	        DeWinterApp.Subscribe<DateTime>(HandleCalendarDay);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<DateTime>(HandleCalendarDay);
	    }

		public void HandleCalendarDay(DateTime day)
	    {
			CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
			DateTime date = day.Date;
			List<Party> parties;
			int i;
			string suffix;
	    	nextParty = null;

			for (i=0; nextParty == null && i<MAX_PARTY_RANGE; i++)
			{
				if (model.Parties.TryGetValue(date.AddDays(i), out parties) && parties.Count > 0)
				{
					nextParty = parties[0];
				}
			}

			if (nextParty != null)
			{
				switch (i)
				{
					case 0:
						suffix = " Party (Today)";
						break;
					case 1:
						suffix = " Party (Tomorrow)";
						break;
					case 2:
						suffix = " Party (The Day After Tomorrow)";
						break;
					default:
						suffix = " Party (" + i + " Days from Now)";
						break;
				}
				myText.text = nextParty.SizeString() + " " + nextParty.faction + suffix;
			}
			else
			{
				myText.text = "";
			}
		}
	}
}