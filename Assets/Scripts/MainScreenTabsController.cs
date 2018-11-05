using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class MainScreenTabsController : MonoBehaviour
	{
	    public GameObject CalendarTab;
	    public GameObject WardrobeTab;
	    public GameObject EstateTab;
	    public GameObject JournalTab;

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
			SetTab(CalendarTab);
	    }

	    public void WardrobeSelected()
	    {
			SetTab(WardrobeTab);
	    }

	    public void EstateSelected()
	    {
            //This tab isn't ready yet, and going there causes issues and looks ugly
            //SetTab(EstateTab);
            AmbitionApp.OpenMessageDialog("feature_incomplete_dialog");
        }

	    public void JournalSelected()
	    {
            //This tab isn't ready yet, and going there causes issues and looks ugly
            //SetTab(JournalTab);
            AmbitionApp.OpenMessageDialog("feature_incomplete_dialog");
        }

        private void HandleTab(string msg)
		{
			switch (msg)
			{
				case EstateConsts.CALENDAR_TAB:
					SetTab(CalendarTab);
					break;
				case EstateConsts.WARDROBE_TAB:
					SetTab(WardrobeTab);
					break;
				case EstateConsts.JOURNAL_TAB:
					SetTab(JournalTab);
					break;
				case EstateConsts.ESTATE_TAB:
					SetTab(EstateTab);
					break;
			}
		}

		private void SetTab(GameObject tab)
		{
			CalendarTab.SetActive(tab == CalendarTab);
			WardrobeTab.SetActive(tab == WardrobeTab);
			EstateTab.SetActive(tab == EstateTab);
			JournalTab.SetActive(tab == JournalTab);
		}
	}
}
