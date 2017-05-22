using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Ambition
{
	public class RoomViewMediator : MonoBehaviour
	{
		// HUD
		public Text RoomTitleText;
		public Image reparteeIndicatorImage;

		// Player
		public Text PlayerNameText;
		public Image PlayerPortrait;
		public TimerView RoomTimer;

		// Art
	    public PartyArtLibrary ArtConfig;

	    protected PartyModel _model;

		protected PartyVO _party
	    {
	    	get { return _model.Party; }
	    }

		protected RemarkVO _remark
	    {
	    	get { return _model.Remark; }
	    }

		// TODO: Passive Buff system
		protected bool _fascinatorEffect=false; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

		void Awake()
		{
			//This is used in the Party Scene to brings up the Conversation/Work the Room Window where the Player combats Guests with their charms
			AmbitionApp.Subscribe(PartyMessages.START_TURN, HandleStartTimer);
			AmbitionApp.Subscribe(PartyMessages.END_TURN, HandleEndTurn);
			RoomTimer.Subscribe(HandleEndTimer);
		}

		void OnDestroy()
	    {
			AmbitionApp.Subscribe(PartyMessages.START_TURN, HandleStartTimer);
			AmbitionApp.Unsubscribe(PartyMessages.END_TURN, HandleEndTurn);
			RoomTimer.Unsubscribe(HandleEndTimer);
	    }

		// Use this for initialization
	    void Start()
	    {
			_model = AmbitionApp.GetModel<PartyModel>();

			RoomTitleText.text = AmbitionApp.GetModel<MapModel>().Room.Name;

			reparteeIndicatorImage.enabled = (GameData.playerReputationLevel >= 2);

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory))
			{
				switch(accessory.Name)
				{
					case "Garter Flask":
						_model.MaxDrinkAmount++;
						break;
					case "Fascinator":
						_fascinatorEffect = true;
						break;
				}
			}

			AmbitionApp.SendMessage<PartyVO>(PartyMessages.PARTY_STARTED, _model.Party);

			//Damage the Outfit's Novelty, how that the Confidence has already been Tallied
			AmbitionApp.SendMessage<Outfit>(InventoryConsts.DEGRADE_OUTFIT, OutfitInventory.PartyOutfit);
	    }

	    // Poll for input
	    void Update()
	    {
			if(Input.GetKeyDown(KeyCode.D))
	        {
	        	AmbitionApp.SendMessage(PartyMessages.DRINK);
	        }
	        else if (Input.GetKeyDown(KeyCode.F))
	        {
				AmbitionApp.SendMessage(PartyMessages.FLIP_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
				AmbitionApp.SendMessage(PartyMessages.BUY_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.E))
	        {
				AmbitionApp.SendMessage(PartyMessages.EXCHANGE_REMARK, _model.Remark);
	        }
        }

		virtual protected void HandleStartTimer()
		{
			// TODO: Clunky as hell. NEED a buff system.
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			float t = _model.TurnTime;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Fan")
			{
				t *= 1.1f;
			}
			RoomTimer.CountDown(t);
		}

		protected void HandleEndTimer(TimerView timer)
		{
			AmbitionApp.SendMessage(PartyMessages.END_TURN);
		}

		protected void HandleEndTurn()
		{
			RoomTimer.Stop();
			if (RoomTimer.Percent <= 0.5f)
			{
				AmbitionApp.SendMessage(PartyMessages.REPARTEE_BONUS);
			}
		}
	}
}
