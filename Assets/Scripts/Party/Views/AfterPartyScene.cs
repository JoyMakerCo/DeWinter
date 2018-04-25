using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Dialog;

namespace Ambition
{
	public class AfterPartyScene : MonoBehaviour
	{
		public const string DIALOG_ID = "AFTER_PARTY_DIALOG";
		public SpriteConfig FactionIconConfig;
		public Text ReputationText;
		public Text GossipText;
		public Text LivreText;
		public Text RewardText;
		public Image RepGainIcon;
		public Image RepLossIcon;
		public Image FactionIcon;
		public Text PartyText;
		public Text NoveltyLossText;
		public Text PartyImportance;

		void Start ()
		{
	        OutfitVO outfit = AmbitionApp.GetModel<GameModel>().Outfit;
			PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
			PartyText.text = party.Name;
			if (party.Host != null) PartyText.text += (" - " + party.Host.Name);
			PartyImportance.text = party.Importance.ToString();
			NoveltyLossText.text = "0";
			FactionIcon.sprite = FactionIconConfig.GetSprite(party.Faction);
			int livre=0;
			int rep = 0;
			foreach (CommodityVO reward in party.Rewards)
			{
				switch(reward.Type)
				{
					case CommodityType.Gossip:
						GossipText.text += "- " + reward.ID + "\n";
						break;
					case CommodityType.Livre:
						livre += reward.Amount;
						break;
					case CommodityType.Reputation:
						rep += reward.Amount;
						break;
					case CommodityType.Item:
						RewardText.text += "- " + reward.ID + "\n";
						break;
				}
			}
			RepGainIcon.enabled = rep > 0;
			RepLossIcon.enabled = rep < 0;
			ReputationText.text = rep.ToString();
			LivreText.text = "£" + livre.ToString();
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeout);
		}

		public void Done()
		{
			AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, HandleFadeout);
			AmbitionApp.SendMessage(GameMessages.FADE_OUT);
		}

		private void HandleFadeout()
		{
			AmbitionApp.SendMessage(CalendarMessages.NEXT_DAY);
			AmbitionApp.InvokeMachine("EstateController");
		}
	}
}
