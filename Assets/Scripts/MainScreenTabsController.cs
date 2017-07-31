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
	    void Start () {
	        CalendarSelected();
		}

	    public void CalendarSelected()
	    {
	        calendarTab.transform.SetAsLastSibling();
	    }

	    public void WardrobeSelected()
	    {
	        wardrobeTab.transform.SetAsLastSibling();
	    }

	    public void EstateSelected()
	    {
	        estateTab.transform.SetAsLastSibling();
	    }

	    public void JournalSelected()
	    {
	        journalTab.transform.SetAsLastSibling();
	    }
	}
}