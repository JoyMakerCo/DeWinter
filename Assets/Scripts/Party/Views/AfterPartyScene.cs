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
        public Image FactionIcon;
        public Text PartyText;
        public Transform RewardContainer;
        public GameObject RewardItem;
        public SpriteConfig RewardIcons;
        public Text OutfitText;
        public Slider NoveltySlider;
        public Text NoveltySliderText;

        private Dictionary<string, AfterPartyRewardItem> _items = new Dictionary<string, AfterPartyRewardItem>();
        private InventoryModel _inventory;
        private 

        void Start()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            _inventory = AmbitionApp.GetModel<InventoryModel>();
            ItemVO[] gossipList = _inventory.GetItems(ItemType.Gossip);
            int gossipIndex = gossipList.Length;
            PartyText.text = model.Party.Name;
            FactionIcon.sprite = FactionIconConfig.GetSprite(model.Party.Faction.ToString());
            model.Party.Rewards.Sort((r1, r2) => r1.Type.CompareTo(r2.Type));

            foreach (CommodityVO reward in model.Party.Rewards)
            {
                switch (reward.Type)
                {
                    case CommodityType.Reputation:
                        if (!string.IsNullOrEmpty(reward.ID))
                        {
                            AddTotal(reward.ID, reward.Value);
                        }
                        else AddTotal(reward.Type.ToString(), reward.Value);
                        break;
                    case CommodityType.Item:
                        break;
                    case CommodityType.Gossip:
                        if (--gossipIndex >= 0)
                            GetGossipItem(gossipList[gossipIndex]);
                        break;
                    case CommodityType.Livre:
                    case CommodityType.Peril:
                    case CommodityType.Credibility:
//                    case CommodityType.Favor:
                        AddTotal(reward.Type.ToString(), reward.Value);
                        break;
                }
            }
            foreach(AfterPartyRewardItem rewardItem in _items.Values)
            {
                rewardItem.UpdateView();
            }
            ItemVO item = _inventory.GetEquippedItem(ItemType.Outfit);
            if (item != null)
            {
                OutfitText.text = item.Name;
                NoveltySlider.value = OutfitWrapperVO.GetNovelty(item);
                NoveltySliderText.text = NoveltySlider.value.ToString();
            }

            string headetTxt = AmbitionApp.GetString("after_party_dialog.title");
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, headetTxt);
        }

        private void AddTotal(string key, int value)
        {
            if (_items.ContainsKey(key))
            {
                _items[key].Value += value;
            }
            else
            {
                GameObject obj = _items.Count == 0 ? RewardItem : Instantiate<GameObject>(RewardItem, RewardContainer);
                obj.SetActive(true);
                _items[key] = obj.GetComponent<AfterPartyRewardItem>();
                _items[key].Value = value;
                _items[key].ShowValue = true;
                _items[key].Text = AmbitionApp.Localize(key) ?? key;
                _items[key].IconImage.sprite = RewardIcons.GetSprite(key.ToString());
            }
        }

        private void GetGossipItem(ItemVO item)
        {
            GameObject obj = Instantiate<GameObject>(RewardItem, RewardContainer);
            AfterPartyRewardItem reward = obj.GetComponent<AfterPartyRewardItem>();
            obj.SetActive(true);
            reward.ShowValue = false;
            reward.Text = item.Name;
            reward.Fluff = GossipWrapperVO.GetDescription(item);
            reward.UpdateView();
        }

        public void Done()
        {
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            AmbitionApp.SendMessage(GameMessages.COMPLETE);
        }
    }
}
