using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Core;

namespace Ambition
{
	public class NoOutfitModal : DialogView
	{
		private const string DIALOG_PHRASE = "no_outfit_dialog";

		public Text BodyText;
		public Text TitleText;
	    private MainScreenTabsController tabsController;

	    public override void OnOpen ()
		{
			base.OnOpen ();
			BodyText.text = AmbitionApp.Localize(DIALOG_PHRASE + DialogConsts.BODY);
			TitleText.text = AmbitionApp.Localize(DIALOG_PHRASE + DialogConsts.TITLE);

	        tabsController = GameObject.Find("MainScreenTabsContainer").GetComponent<MainScreenTabsController>();
	    }

	    public void CreateCancellationModal()
	    {
	    	CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            PartyModel mdoel = AmbitionApp.GetModel<PartyModel>();
            OccasionVO[] occasions = calendar.GetOccasions(OccasionType.Party);
            foreach(OccasionVO occasion in occasions)
            {
                if (mdoel.Parties.TryGetValue(occasion.ID, out PartyVO party) && party.Attending)
                {
                    AmbitionApp.OpenDialog(DialogConsts.CANCEL, party);
                    return;
                }
            }
	    }

        public void GoToTheMerchant() => tabsController.WardrobeSelected();
	}
}