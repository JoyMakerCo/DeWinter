using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class RewardRangeListItem : SortableItem<RewardRange>
    {
        public Text RangeText;
        public Image IconImage;
        public SpriteConfig IconConfig;

        public override RewardRange Data
        {
            get => base.Data;
            set
            {
                Sprite sprite = null;
                string text = null;
                string valueStr = (value.Low != value.High
                    ? RangeStr(value.Low) + "–" + RangeStr(value.High)
                    : RangeStr(value.Low));
                Dictionary<string, string> subs = new Dictionary<string, string>();
                base.Data = value;

                switch (value.Type)
                {
                    case CommodityType.Gossip:
                        CommodityVO gossip = new CommodityVO(CommodityType.Gossip, value.ID, value.Low);
                        text = AmbitionApp.GetModel<GossipModel>().GetName(gossip);
                        sprite = IconConfig.GetSprite(value.ID.ToLower());
                        break;
                    case CommodityType.Servant:
                        text = AmbitionApp.Localize("servant.reward." + value.ID);
                        sprite = IconConfig.GetSprite(value.ID.ToLower());
                        break;
                    case CommodityType.Favor:
                        subs["$SHORTNAME"] = AmbitionApp.Localize(CharacterConsts.LOC_SHORT_NAME + value.ID);
                        text = valueStr + " " + AmbitionApp.Localize("favor.reward", subs);
                        sprite = IconConfig.GetSprite(value.ID);
                        break;
                }
                IconImage.sprite = sprite ?? IconConfig.GetSprite(value.Type.ToString().ToLower());
                if (text != null) RangeText.text = text;
                else RangeText.text = valueStr + " " + AmbitionApp.Localize(value.Type.ToString().ToLower());
            }
        }

        private string RangeStr(int val)
        {
            int tag = 0;
            while (Math.Abs(val) >= 1000)
            {
                val = (int)Math.Floor(val*.001);
                ++tag;
            }
            switch(tag)
            {
                case 0:
                    return val.ToString();
                case 1:
                    return val.ToString() + "K";
            }
            return val.ToString() + "M";
        }
    }
}
