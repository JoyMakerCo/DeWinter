using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Core;

namespace DeWinter
{
	public class NoOutfitModal : DialogView
	{
		public const string DIALOG_ID = "NoOutfitModal";
		private const string DIALOG_PHRASE = "no_outfit_dialog";

		public Text BodyText;
		public Text TitleText;
	    private MainScreenTabsController tabsController;

	    void Start()
	    {
	    	LocalizationModel localization = DeWinterApp.GetModel<LocalizationModel>();
			BodyText.text = localization.GetString(DIALOG_PHRASE + DialogConsts.BODY);
			TitleText.text = localization.GetString(DIALOG_PHRASE + DialogConsts.TITLE);

	        tabsController = GameObject.Find("MainScreenTabsContainer").GetComponent<MainScreenTabsController>();
	    }

	    public void CreateCancellationModal()
	    {
	    	List<Party> parties;
	    	CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
	    	if (cmod.Parties.TryGetValue(cmod.Today, out parties))
	    	{
	    		Party party = parties.Find(p => p.RSVP == 1);
	    		if (party != null)
	    		{
					DeWinterApp.OpenDialog<Party>(RSVPCancelDialogMediator.DIALOG_ID, party);
				}
			}
	    }

	    public void GoToTheMerchant()
	    {
	        tabsController.WardrobeSelected();
	    }
	}
}