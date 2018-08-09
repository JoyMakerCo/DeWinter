using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class MainScreenTabsController : MonoBehaviour
	{
	    public GameObject calendarTab;
	    public GameObject wardrobeTab;
	    public GameObject estateTab;
	    public GameObject journalTab;

	    // Use this for initialization
	    void Start ()
		{
	        CalendarSelected();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<string>(EstateMessages.OPEN_TAB, HandleTab);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<string>(EstateMessages.OPEN_TAB, HandleTab);
		}

	    public void CalendarSelected()
	    {
			SetTab(calendarTab);
	    }

	    public void WardrobeSelected()
	    {
			SetTab(wardrobeTab);
	    }

	    public void EstateSelected()
	    {
			SetTab(estateTab);
	    }

	    public void JournalSelected()
	    {
			SetTab(journalTab);
	    }

		private void HandleTab(string msg)
		{
			switch (msg)
			{
				case EstateConsts.CALENDAR_TAB:
					SetTab(calendarTab);
					break;
				case EstateConsts.WARDROBE_TAB:
					SetTab(wardrobeTab);
					break;
				case EstateConsts.JOURNAL_TAB:
					SetTab(journalTab);
					break;
				case EstateConsts.ESTATE_TAB:
					SetTab(estateTab);
					break;
			}
		}

		private void SetTab(GameObject tab)
		{
			calendarTab.SetActive(tab == calendarTab);
			wardrobeTab.SetActive(tab == wardrobeTab);
			estateTab.SetActive(tab == estateTab);
			journalTab.SetActive(tab == journalTab);
		}
	}
}
