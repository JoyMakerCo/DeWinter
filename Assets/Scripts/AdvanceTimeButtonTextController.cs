using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class AdvanceTimeButtonTextController : MonoBehaviour
	{
	    private Text _text;
	    private PartyVO _party;
	    private DateTime _date;

	    void Start ()
	    {
	        _text = this.GetComponentInChildren<Text>();
	        AmbitionApp.Subscribe<PartyVO>(PartyMessages.RSVP, HandleRSVP);
			AmbitionApp.Subscribe<DateTime>(HandleDay);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.RSVP, HandleRSVP);
			AmbitionApp.Unsubscribe<DateTime>(HandleDay);
	    }

	    private void HandleDay(DateTime date)
	    {
			PartyVO p = AmbitionApp.GetModel<PartyModel>().Party;
	    	_date = date;
	    	if (p != null) HandleRSVP(p);
	    }

		private void HandleRSVP (PartyVO party)
	    {
			if (party.Date == _date && party.RSVP > 0) _party = party;
			else if (party == _party && party.RSVP < 0) _party = null;
			_text.text = (_party != null) ? "Go to the Party!" : "Next Day";
		}

		public void OnClick()
		{
			if (_party == null)
			{
				AmbitionApp.SendMessage(CalendarMessages.NEXT_DAY);
				AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_ESTATE);
			}
			else if (OutfitInventory.personalInventory.Count <= 0)
			{
				//You ain't got no clothes to attend the party! 
                AmbitionApp.OpenDialog(DialogConsts.NO_OUTFIT);
			}
			else
			{
				// Go to the party
				AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT);
			}
		}
	}
}