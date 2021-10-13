using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Dialog;

namespace Ambition
{
    public class AfterPartyScene : SceneView, ISubmitHandler
    {
        public const string DIALOG_ID = "AFTER_PARTY_DIALOG";
        public const string HOST_TOKEN = "$HOST";

        public SpriteConfig FactionIconConfig;
        public Image FactionIcon;
        public Text PartyText;
        public RewardItem listItem;
        public Text OutfitText;
        public Slider NoveltySlider;
        public Text NoveltySliderText;
        public Text PartyHostText;

        private Dictionary<string, RewardItem> _items = new Dictionary<string, RewardItem>();
        private InventoryModel _inventory;

        void Start()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            _inventory = AmbitionApp.GetModel<InventoryModel>();
            List<GossipVO> gossipList = AmbitionApp.Gossip.Gossip;
            int gossipIndex = gossipList.Count-1;
            PartyText.text = AmbitionApp.Localize(PartyConstants.PARTY_NAME + model.Party.ID);
            FactionIcon.sprite = FactionIconConfig.GetSprite(model.Party.Faction.ToString());
            List<RewardItem> rewards = AmbitionApp.CreateRewardListItems(model.Rewards, listItem);
            OutfitVO outfit = _inventory.GetEquippedItem(ItemType.Outfit) as OutfitVO;
            if (outfit != null)
            {
                int novelty = outfit.Novelty - model.BaseNoveltyLoss - model.CumulativeNoveltyLoss * outfit.TimesWorn;
                if (novelty < 0) novelty = 0;
                OutfitText.text = AmbitionApp.Localization.GetItemName(outfit);
                NoveltySlider.value = novelty;
                NoveltySliderText.text = novelty.ToString();
            }
            string host = AmbitionApp.Localize(PartyConstants.PARTY_HOST + model.Party.Host);
            if (string.IsNullOrEmpty(host)) host = model.Party.Host;
            Dictionary<string, string> subs = new Dictionary<string, string>() { { HOST_TOKEN, host } };
            PartyHostText.text = AmbitionApp.Localize("after_party_dialog.host", subs);
            for (int i=rewards.Count-1; i>=0; --i)
            {
                if (rewards[i].Data.Type == CommodityType.Gossip)
                {
                    rewards[i].SetGossip(gossipList[gossipIndex]);
                    --gossipIndex;
                }
            }
        }

        public void Cancel()
        {
#if UNITY_STANDALONE
            AmbitionApp.OpenDialog(DialogConsts.GAME_MENU);
#else
            AmbitionApp.SendMessage(GameMessages.COMPLETE);
#endif
        }
        public void Submit() => AmbitionApp.SendMessage(GameMessages.COMPLETE);
    }
}
