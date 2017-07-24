using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class PartyTracker : MonoBehaviour
	{
		private const int MAX_PARTY_RANGE = 15;
	    private Text myText;
	    public PartyVO nextParty;

	    void Start()
	    {
	        myText = GetComponent<Text>();
	        AmbitionApp.Subscribe<DateTime>(HandleCalendarDay);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<DateTime>(HandleCalendarDay);
	    }

		public void HandleCalendarDay(DateTime day)
	    {
			CalendarModel model = AmbitionApp.GetModel<CalendarModel>();
			DateTime date = day.Date;
			List<PartyVO> parties;
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
				myText.text = nextParty.SizeString() + " " + nextParty.Faction + suffix;
			}
			else
			{
				myText.text = "";
			}
		}
	}
}