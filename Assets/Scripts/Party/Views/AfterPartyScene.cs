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
		public Text NoveltyLossValueText;
		public Text PartyImportanceValueText;

		void Start ()
		{
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
			PartyText.text = model.Party.Name;
            PartyImportanceValueText.text = model.Party.Size.ToString();
            NoveltyLossValueText.text = "-" + AmbitionApp.GetModel<InventoryModel>().NoveltyDamage.ToString();
			FactionIcon.sprite = FactionIconConfig.GetSprite(model.Party.Faction.ToString());
            //Setting all these values to blank to zero in order to override the placeholder stuff
            int livre = 0;
			int rep = 0;
            RewardText.text = "";
            GossipText.text = "";
            foreach (CommodityVO reward in model.Party.Rewards)
			{
				switch(reward.Type)
				{
					case CommodityType.Gossip:
						GossipText.text += "- " + AmbitionApp.Localize("commodity." + reward.Type.ToString().ToLower() + ".name") + "\n";
                        print("Gossip Item Added!");
						break;
					case CommodityType.Livre:
						livre += reward.Value;
						break;
					case CommodityType.Reputation:
						rep += reward.Value;
						break;
					case CommodityType.Item:
						RewardText.text += "- " + AmbitionApp.Localize("commodity." + reward.ID.ToLower() + ".name") + "\n";
						break;
				}
			}
            if (GossipText.text == "") GossipText.text = AmbitionApp.Localize("commodity.none.name");
            if (RewardText.text == "") RewardText.text = AmbitionApp.Localize("commodity.none.name");
            RepGainIcon.enabled = rep >= 0;
			RepLossIcon.enabled = rep < 0;
            if(rep >= 0)
            {
                ReputationText.text = "+" + rep.ToString();
            } else
            {
                ReputationText.text = rep.ToString();
            }
			LivreText.text = "+ £" + livre.ToString();
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
		}

		public void Done() => AmbitionApp.SendMessage(PartyMessages.END_PARTY);
	}
}
