using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class AdvanceTimeButtonTextController : MonoBehaviour
	{
	    private Text _text;
	    private DateTime _date;
	    private Button _btn;
		private bool _goParty;

	    void Awake()
	    {
			_btn = this.GetComponent<Button>();
			_btn.onClick.AddListener(OnClick);
			_text = this.GetComponentInChildren<Text>();
			AmbitionApp.Subscribe<PartyVO>(HandleParty);
			AmbitionApp.Subscribe<DateTime>(HandleDay);
			_date = AmbitionApp.GetModel<CalendarModel>().Today;
			SetMode(AmbitionApp.GetModel<PartyModel>().Party);
	    }

	    void OnDestroy()
	    {
			_btn.onClick.RemoveListener(OnClick);
			AmbitionApp.Unsubscribe<PartyVO>(HandleParty);
			AmbitionApp.Unsubscribe<DateTime>(HandleDay);
	    }

	    private void HandleDay(DateTime date)
	    {
	    	_date = date;
	    }

		private void HandleParty (PartyVO party)
	    {
			if (party == null || party.Date == _date)
			{
				SetMode(party);
			}
		}

		private void SetMode(PartyVO party)
		{
			_goParty = party != null && party.Date == _date && party.RSVP > 0;
			_text.text = _goParty ? "Go to the Party!" : "Next Day";
		}

		private void OnClick()
		{
			if (!_goParty)
			{
                AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.PARIS_SCENE);
			}
			else
			{
				InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
				if (model.Inventory.Exists(i=>i.Type == ItemConsts.OUTFIT))
				{
					// Go to the party
					AmbitionApp.SendMessage<string>(GameMessages.LOAD_SCENE, SceneConsts.LOAD_OUT_SCENE);
				}
				else 
				{
					//You ain't got no clothes to attend the party! 
	                AmbitionApp.OpenDialog(DialogConsts.NO_OUTFIT);
               	}
			}
		}
	}
}
