using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class GossipInventoryListItem : SortableItem<GossipVO>
    {
        public Text NameText;
        public Image FactionIcon;
        public RawImage SelectedImage;
        public SpriteConfig FactionConfig;
        public Image QuestCompleteIcon;

        private GossipVO _gossip;
        public override GossipVO Data
        {
            get => _gossip;
            set
            {
                GossipModel gossip = AmbitionApp.Gossip;
                bool completeQuest = gossip.Quests.Exists(q => q.Faction == value.Faction);
                _gossip = value;
                NameText.text = gossip.GetName(_gossip);
                FactionIcon.sprite = FactionConfig.GetSprite(_gossip.Faction.ToString());
                QuestCompleteIcon.gameObject.SetActive(completeQuest);
            }
        }

        private void Awake()
        {
            AmbitionApp.Subscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, HandleGossip);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<GossipVO>(InventoryMessages.DISPLAY_GOSSIP, HandleGossip);
        }

        public void OnClick() => AmbitionApp.SendMessage(InventoryMessages.DISPLAY_GOSSIP, _gossip);

        public void HandleGossip(GossipVO gossip)
        {
            SelectedImage.enabled = (_gossip == gossip);
        }
    }
}
