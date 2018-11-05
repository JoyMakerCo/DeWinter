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
        Core.LocalizationModel localizationModel = AmbitionApp.GetModel<Core.LocalizationModel>();
        InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();

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
			PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
			PartyText.text = party.Name;
			if (party.Host != null) PartyText.text += (" - " + party.Host);
            PartyImportanceValueText.text = party.Importance.ToString();
            NoveltyLossValueText.text = "-" + inventory.NoveltyDamage.ToString();
			FactionIcon.sprite = FactionIconConfig.GetSprite(party.Faction);
            //Setting all these values to blank to zero in order to override the placeholder stuff
            int livre = 0;
			int rep = 0;
            RewardText.text = "";
            GossipText.text = "";
            foreach (CommodityVO reward in party.Rewards)
			{
				switch(reward.Type)
				{
					case CommodityType.Gossip:
						GossipText.text += "- " + localizationModel.GetString("commodity." + reward.Type.ToString().ToLower() + ".name") + "\n";
                        print("Gossip Item Added!");
						break;
					case CommodityType.Livre:
						livre += reward.Value;
						break;
					case CommodityType.Reputation:
						rep += reward.Value;
						break;
					case CommodityType.Item:
						RewardText.text += "- " + localizationModel.GetString("commodity." + reward.ID.ToLower() + ".name") + "\n";
						break;
				}
			}
            if (GossipText.text == "") GossipText.text = localizationModel.GetString("commodity.none.name");
            if (RewardText.text == "") RewardText.text = localizationModel.GetString("commodity.none.name");
            RepGainIcon.enabled = rep >= 0;
			RepLossIcon.enabled = rep < 0;
            if(rep >= 0)
            {
                ReputationText.text = "+";
            } else
            {
                ReputationText.text = "";
            }
            ReputationText.text += rep.ToString();
			LivreText.text = "+ £" + livre.ToString();
		}

		public void Done()
		{
			AmbitionApp.SendMessage(PartyMessages.END_PARTY);
            AmbitionApp.GetModel<PartyModel>().Party = null;
        }
	}
}
