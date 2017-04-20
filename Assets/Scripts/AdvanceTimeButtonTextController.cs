using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class AdvanceTimeButtonTextController : MonoBehaviour
	{
	    private Text _text;
	    private Party _party;
	    private DateTime _date;

	    void Start ()
	    {
	        _text = this.GetComponentInChildren<Text>();
	        DeWinterApp.Subscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Subscribe<DateTime>(HandleDay);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<Party>(PartyMessages.RSVP, HandleRSVP);
			DeWinterApp.Unsubscribe<DateTime>(HandleDay);
	    }

	    private void HandleDay(DateTime date)
	    {
			Party p = DeWinterApp.GetModel<PartyModel>().Party;
	    	_date = date;
	    	if (p != null) HandleRSVP(p);
	    }

		private void HandleRSVP (Party party)
	    {
			if (party.Date == _date && party.RSVP > 0) _party = party;
			else if (party == _party && party.RSVP < 0) _party = null;
			_text.text = (_party != null) ? "Go to the Party!" : "Next Day";
		}

		public void OnClick()
		{
			if (_party == null)
			{
				DeWinterApp.SendMessage(CalendarMessages.NEXT_DAY);
				DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_ESTATE);
			}
			else if (OutfitInventory.personalInventory.Count <= 0)
			{
				//You ain't got no clothes to attend the party! 
                DeWinterApp.OpenDialog(NoOutfitModal.DIALOG_ID);
			}
			else
			{
				// Go to the party
				DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.GAME_PARTYLOADOUT);
			}
		}
	}
}