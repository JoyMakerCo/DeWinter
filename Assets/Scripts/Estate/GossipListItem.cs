using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class GossipListItem : SortableItem<GossipVO>
    {
        public SpriteConfig FactionSprites;
        public Text NameText;
        public Text DescriptionText;
        public Text ValueText;
        public Text RelevanceText;
        public Image FactionIcon;

        private GossipVO _gossip;

        public override GossipVO Data
        {
            get => _gossip;
            set
            {
                Dictionary<string, string> subs = new Dictionary<string, string>();
                int age = AmbitionApp.Calendar.Day - value.Created;
                subs["%R"] = AmbitionApp.Localize(GossipConsts.GOSSIP_RELEVANCE_LOC + AmbitionApp.Gossip.GetRelevanceIndex(age));
                subs["%N"] = age.ToString();
                subs["%D"] = AmbitionApp.Localize(age == 1 ? LocalizationConsts.Day : LocalizationConsts.Days);
                _gossip = value;
                NameText.text = AmbitionApp.Gossip.GetName(_gossip);
                DescriptionText.text = AmbitionApp.Gossip.GetDescription(_gossip);
                ValueText.text = "£" + AmbitionApp.Gossip.GetValue(_gossip, AmbitionApp.Calendar.Day).ToString("### ###");
                RelevanceText.text = AmbitionApp.Localize(EstateConsts.RELEVANCE_LOC, subs);
                StartCoroutine(Rebuild());
                FactionIcon.sprite = FactionSprites.GetSprite(_gossip.Faction.ToString());
            }
        }

        IEnumerator Rebuild()
        {
            for (float i=0; i==0; i+=Time.deltaTime)
            {
                yield return null;
            }
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.parent.GetComponent<UnityEngine.RectTransform>());
        }
    }
}
