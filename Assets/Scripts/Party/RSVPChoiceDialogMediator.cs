using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Dialog;
using Core;

namespace DeWinter
{
	public class RSVPChoiceDialogMediator : DialogView, IDialog<List<Party>>
	{
		public Text BodyText;
		public Text TitleText;
	    private List<Party> _parties;

	    // Use this for initialization
	    public void OnOpen (List<Party> parties)
	    {
	    	LocalizationModel localization = DeWinterApp.GetModel<LocalizationModel>();
	    	_parties = parties;

			TitleText.text = localization.GetString(DialogConsts.RSVP_CHOICE_DIALOG + DialogConsts.TITLE);
	    	BodyText.text = localization.GetString(DialogConsts.RSVP_CHOICE_DIALOG + DialogConsts.BODY);

	        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
	        party1ButtonText.text = _parties[0].Name();
	        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
			party2ButtonText.text = _parties[1].Name();
	    }

	    public void SelectParty(int partyNumber)
	    {
	    	for (int i=_parties.Count-1; i >= 0; i--)
	    	{
	    		if (_parties[i] != null)
	    		{
	    			if (i != partyNumber)
	    			{
	    				if (_parties[i].RSVP > 0)
	    				{
							DeWinterApp.OpenDialog<Party>("CancellationModal", _parties[i]);
		    			}
		    			else
		    			{
		    				_parties[i].RSVP = -1;
							DeWinterApp.SendMessage(PartyMessages.RSVP, _parties[i]);
		    			}
		    		}
	    			else
	    			{
						DeWinterApp.SendMessage<Party>("RSVPPopUpModal", _parties[i]);
	    			}
	    		}
	    	}
	    }
	}
}