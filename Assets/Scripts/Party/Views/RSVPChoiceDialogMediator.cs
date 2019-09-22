using System.Collections.Generic;
using UnityEngine.UI;
using Dialog;
using Core;

namespace Ambition
{
	public class RSVPChoiceDialogMediator : DialogView<List<PartyVO>>
	{
		public Text BodyText;
		public Text TitleText;
	    private List<PartyVO> _parties;

	    // Use this for initialization
	    public override void OnOpen (List<PartyVO> parties)
	    {
	    	_parties = parties;

			TitleText.text = AmbitionApp.GetString(DialogConsts.RSVP_CHOICE_DIALOG + DialogConsts.TITLE);
	    	BodyText.text = AmbitionApp.GetString(DialogConsts.RSVP_CHOICE_DIALOG + DialogConsts.BODY);

	        Text party1ButtonText = this.transform.Find("Party1Button").Find("Text").GetComponent<Text>();
	        party1ButtonText.text = _parties[0].Name;
	        Text party2ButtonText = this.transform.Find("Party2Button").Find("Text").GetComponent<Text>();
			party2ButtonText.text = _parties[1].Name;
	    }

	    public void SelectParty(int partyNumber)
	    {
	    	for (int i=_parties.Count-1; i >= 0; i--)
	    	{
	    		if (_parties[i] != null)
	    		{
	    			if (i != partyNumber)
	    			{
                        if (_parties[i].RSVP == RSVP.Accepted)
	    				{
							AmbitionApp.OpenDialog(DialogConsts.RSVP_CHOICE, _parties[i]);
		    			}
		    			else
		    			{
                            _parties[i].RSVP = RSVP.Declined;
							AmbitionApp.SendMessage(_parties[i]);
		    			}
		    		}
	    			else
	    			{
						AmbitionApp.SendMessage(DialogConsts.RSVP, _parties[i]);
	    			}
	    		}
	    	}
	    }
	}
}