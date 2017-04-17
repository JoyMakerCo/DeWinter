using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;

namespace DeWinter
{
	public class NoOutfitModal : MonoBehaviour
	{
	    private MainScreenTabsController tabsController;

	    void Start()
	    {
	        tabsController = GameObject.Find("MainScreenTabsContainer").GetComponent<MainScreenTabsController>();
	    }

	    public void Dismiss()
	    {
	        Destroy(transform.parent.gameObject);
	    }

	    public void CreateCancellationModal()
	    {
	    	List<Party> parties;
	    	DateTime today = DeWinterApp.GetModel<CalendarModel>().Today;
	    	if (DeWinterApp.GetModel<CalendarModel>().Parties.TryGetValue(today, out parties))
	    	{
	    		Party party = parties.Find(p => p.RSVP == 1);
	    		if (party != null)
	    		{
					DeWinterApp.OpenDialog<Party>("CancellationModal", party);
				}
			}
	    }

	    public void GoToTheMerchant()
	    {
	        tabsController.WardrobeSelected();
	    }
	}
}