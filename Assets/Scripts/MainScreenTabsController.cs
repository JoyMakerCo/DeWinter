using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainScreenTabsController : MonoBehaviour {

    public GameObject calendarTab;
    public GameObject calendarButton;
    public GameObject wardrobeTab;
    public GameObject wardrobeButton;
    public GameObject estateTab;
    public GameObject estateButton;
    public GameObject journalTab;
    public GameObject journalButton;

    // Use this for initialization
    void Start () {
        CalendarSelected();
	}

    public void CalendarSelected()
    {
        calendarTab.transform.SetAsLastSibling();
        calendarButton.GetComponent<Image>().color = Color.black;
        wardrobeButton.GetComponent<Image>().color = Color.white;
        estateButton.GetComponent<Image>().color = Color.white;
        journalButton.GetComponent<Image>().color = Color.white;
    }

    public void WardrobeSelected()
    {
        wardrobeTab.transform.SetAsLastSibling();
        wardrobeButton.GetComponent<Image>().color = Color.black;
        calendarButton.GetComponent<Image>().color = Color.white;
        estateButton.GetComponent<Image>().color = Color.white;
        journalButton.GetComponent<Image>().color = Color.white;
    }

    public void EstateSelected()
    {
        estateTab.transform.SetAsLastSibling();
        estateButton.GetComponent<Image>().color = Color.black;
        calendarButton.GetComponent<Image>().color = Color.white;
        wardrobeButton.GetComponent<Image>().color = Color.white;
        journalButton.GetComponent<Image>().color = Color.white;
    }

    public void JournalSelected()
    {
        journalTab.transform.SetAsLastSibling();
        journalButton.GetComponent<Image>().color = Color.black;
        calendarButton.GetComponent<Image>().color = Color.white;
        wardrobeButton.GetComponent<Image>().color = Color.white;
        estateButton.GetComponent<Image>().color = Color.white;
    }
}
