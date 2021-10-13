using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Ambition
{
    public class QuestListItem : SortableItem<QuestVO>
    {
        public SpriteConfig FactionConfig;
        public SpriteConfig RewardConfig;
        public Image FactionSymbol;
        public Text Name;
        public Text DaysLeft;
        public Image RewardIcon;
        public Text RewardText;
        public Image CompletedIcon;

        public override QuestVO Data
        {
            get => _quest;
            set
            {
                Sprite sprite = null;
                _quest = value;
                gameObject.SetActive(_quest != null);
                if (_quest != null)
                {
                    Dictionary<string, string> subs = new Dictionary<string, string>();
                    subs["%N"] = (_quest.Due - AmbitionApp.Calendar.Day).ToString();
                    subs["%F"] = AmbitionApp.Localize(_quest.Faction.ToString().ToLower());
                    Name.text = AmbitionApp.Localize("quest", subs);
                    DaysLeft.text = AmbitionApp.Localize("quest.days_left", subs);
                    FactionSymbol.sprite = FactionConfig.GetSprite(_quest.Faction.ToString());
                    RewardText.text = (_quest.Reward.Type == CommodityType.Livre)
                        ? "£" + _quest.Reward.Value.ToString("###")
                        : _quest.Reward.Value.ToString("###");
                    switch(_quest.Reward.Type)
                    {
                        case CommodityType.Gossip:
                            sprite = RewardConfig.GetSprite(_quest.Reward.ID.ToLower());
                            break;
                        case CommodityType.Favor:
                            sprite = RewardConfig.GetSprite(_quest.Reward.ID);
                            break;
                    }
                    RewardIcon.sprite = sprite ?? RewardConfig.GetSprite(_quest.Reward.Type.ToString().ToLower());
                    CompletedIcon.enabled = AmbitionApp.Gossip.Gossip.Exists(g => g.Faction == _quest.Faction);
                }
            }
        }
        private QuestVO _quest;
    }

    public class QuestList : SortableList<QuestVO>
    {
        protected override QuestVO[] FetchListData() => AmbitionApp.Gossip.Quests.ToArray();
    }
}
