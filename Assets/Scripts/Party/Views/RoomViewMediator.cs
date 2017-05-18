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
		private RemarkVO _remark=null;
		private float _percentTimerComplete;

		public Image reparteeIndicatorImage;

	    public Image readyGoPanel;
	    public Text readyGoText;
	    public PartyArtLibrary ArtConfig;

		// TODO: Passive Buff system
		private bool _fascinatorEffect=false; //The Fascinator Accessory lets the first negative comment go ignored during each Conversation

		void Awake()
		{
			//This is used in the Party Scene to brings up the Conversation/Work the Room Window where the Player combats Guests with their charms
			AmbitionApp.Subscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			AmbitionApp.Subscribe<RemarkVO>(HandleRemarkSelected);
			AmbitionApp.Subscribe(PartyMessages.END_TURN, HandleEndTurn);
		}

		void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<float>(PartyMessages.START_TIMERS, HandleTimers);
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemarkSelected);
			AmbitionApp.Unsubscribe(PartyMessages.END_TURN, HandleEndTurn);
	    }

		// Use this for initialization
	    void Start()
	    {
			PartyModel model = AmbitionApp.GetModel<PartyModel>();

			reparteeIndicatorImage.enabled = (GameData.playerReputationLevel >= 2);

	        //Ready Go Text
	        // TODO: Put all of this into the localization file
			string[] conversationIntroList = model.ConversationIntros;
	        readyGoText.text = conversationIntroList[new System.Random().Next(conversationIntroList.Length)];

	        //Is the Player using the Fascinator Accessory? If so then allow them to ignore the first negative comment!
	        // TODO: Passive buff system
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;

			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory))
			{
				switch(accessory.Name)
				{
					case "Garter Flask":
						model.MaxDrinkAmount++;
						break;
					case "Fascinator":
						_fascinatorEffect = true;
						break;
				}
			}

			AmbitionApp.SendMessage<PartyVO>(PartyMessages.PARTY_STARTED, model.Party);

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
	        	FlipTargets();
	        }
			else if (Input.GetKeyDown(KeyCode.C))
	        {
				AmbitionApp.SendMessage(PartyMessages.BUY_REMARK);
	        }
			else if (Input.GetKeyDown(KeyCode.E))
	        {
				AmbitionApp.SendMessage(PartyMessages.EXCHANGE_REMARK, _remark);
	        }
        }

		private void HandleRemarkSelected(RemarkVO remark)
		{
			_remark = remark;
		}
	
		public void FlipTargets()
	    {
			int flip = _remark.Profile;
			_remark.Profile = 0;
	    	while (flip > 0)
	    	{
	    		_remark.Profile <<= 1;
	    		_remark.Profile += (flip & 1);
				flip >>= 1;
	    	}
	    	AmbitionApp.SendMessage<GuestVO>(PartyMessages.GUEST_SELECTED, null);
	    	AmbitionApp.SendMessage<RemarkVO>(_remark);
	    }

		private void HandleTimers(float seconds)
		{
			StartCoroutine(TurnTimerCoroutine(seconds));
		}

		IEnumerator TurnTimerCoroutine(float seconds)
		{
// TODO: Add this to the Room timer view
			for (float t=0; t < seconds; t += Time.deltaTime)
			{
				_percentTimerComplete = t/seconds;
				yield return null;
			}
			_percentTimerComplete = 1.0f;
			AmbitionApp.SendMessage(PartyMessages.END_TURN);
		}

		void HandleEndTurn()
	    {
	    	StopAllCoroutines();
			if (_percentTimerComplete < 0.5f)
		    	AmbitionApp.SendMessage(PartyMessages.REPARTEE_BONUS);
	    }	  
	}
}