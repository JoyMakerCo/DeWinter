using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RewardItem : SortableItem<CommodityVO>
    {
        public Image IconImage;
        public Text TypeText;
        public Text FluffText;
        public SpriteConfig IconConfig;

        private CommodityVO _data; 
        public override CommodityVO Data
        {
            get => _data;
            set
            {
                Sprite sprite = null;
                string text = null;
                Dictionary<string, string> subs = new Dictionary<string, string>();
                _data = value;
                FluffText.enabled = false;
                if (_data != null)
                {
                    switch (_data?.Type)
                    {
                        case CommodityType.Favor:
                            sprite = IconConfig.GetSprite(_data.ID);
                            subs["$SHORTNAME"] = AmbitionApp.Localize(CharacterConsts.LOC_SHORT_NAME + _data.ID) ?? _data.ID;
                            text = _data.Value + " " + AmbitionApp.Localize("favor.reward", subs);
                            break;
                        case CommodityType.Gossip:
                            text = AmbitionApp.GetModel<GossipModel>().GetName(_data);
                            break;
                    }
                    TypeText.text = text ?? value.Value.ToString("### ###") + " " + AmbitionApp.Localize(value.Type.ToString().ToLower());
                    IconImage.sprite = sprite ?? IconConfig.GetSprite(_data.Type.ToString().ToLower());
                }
            }
        }

        public void SetGossip(GossipVO gossip)
        {
            GossipModel model = AmbitionApp.GetModel<GossipModel>();
            IconImage.sprite = IconConfig.GetSprite(gossip.Faction.ToString().ToLower());
            TypeText.text = model.GetName(gossip);
            FluffText.enabled = true;
            FluffText.text = model.GetDescription(gossip);
            _data = new CommodityVO(CommodityType.Gossip, gossip.Faction.ToString(), gossip.Tier);
        }
    }
}