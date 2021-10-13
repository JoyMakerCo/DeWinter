using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;
using UnityEngine;

namespace Ambition
{
    [Saveable]
    public class GossipModel : ObservableModel<GossipModel>, IResettable
    {
        [JsonProperty("quests")]
        public List<QuestVO> Quests = new List<QuestVO>();

        [JsonProperty("gossip")]
        public List<GossipVO> Gossip = new List<GossipVO>();

        [JsonProperty("gossip_activity")]
        private int _gossipActivity;
        public int GossipActivity
        {
            get => _gossipActivity;
            set
            {
                _gossipActivity = value < 0 ? 0
                    : value < ReproachChance.Length ? value
                    : ReproachChance.Length - 1;
            }
        }

        [JsonIgnore]
        public Relevance[] Relevances;

        [JsonIgnore]
        public GossipTier[] Tiers;

        [JsonIgnore]
        public int[] ReproachChance;

        [JsonIgnore]
        public string[] ReproachIncidents;

        [JsonIgnore]
        public int MaxQuests;

        [JsonIgnore]
        public RewardTier[] RewardTiers;

        public Relevance GetRelevance(int age) => Relevances[GetRelevanceIndex(age)];
        public int GetRelevanceIndex(int age)
        {
            for (int i=Relevances.Length - 1; i>0; --i)
            {
                if (age >= Relevances[i].Age) return i;
            }
            return 0;
        }

        public string GetName(QuestVO quest)
        {
            Dictionary<string, string> subs = new Dictionary<string, string>() {{"$F",quest.Faction.ToString()}};
            return AmbitionApp.Localize(GossipConsts.QUEST_NAME_LOC, subs);
        }

        public string GetName(GossipVO gossip)
        {
            string faction = AmbitionApp.Localize(gossip.Faction.ToString().ToLower());
            string tier = AmbitionApp.Localize(GossipConsts.GOSSIP_TIER_LOC + gossip.Tier);
            Dictionary<string, string> subs = new Dictionary<string, string>()
            {{"$F",faction},
            {"$T",tier}};
            return AmbitionApp.Localize(GossipConsts.GOSSIP_NAME_LOC, subs);
        }

        public string GetName(CommodityVO reward)
        {
            string faction = AmbitionApp.Localize(reward.ID.ToLower());
            string tier = AmbitionApp.Localize(GossipConsts.GOSSIP_TIER_LOC + reward.Value);
            Dictionary<string, string> subs = new Dictionary<string, string>()
            {{"$F",faction},
            {"$T",tier}};
            return AmbitionApp.Localize(GossipConsts.GOSSIP_NAME_LOC, subs);
        }

        public string GetDescription(GossipVO gossip)
        {
            return AmbitionApp.Localize("gossip." + gossip.Tier + "." + gossip.Faction.ToString().ToLower() + (gossip.IsPower ? ".power" : ".allegiance"));
        }

        public int GetValue(GossipVO gossip, int day)
        {
            if (gossip == null || gossip.Tier >= Tiers.Length) return 0;
            int result = (int)(GetRelevance(day - gossip.Created).PriceMultiplier * Tiers[gossip.Tier].Price);
            return result > 1 ? result : 1;
        }

        public int GetShift(GossipVO gossip, int day)
        {
            if (gossip == null || gossip.Tier >= Tiers.Length) return 0;
            int effect = gossip.IsPower ? Tiers[gossip.Tier].Power : Tiers[gossip.Tier].Allegiance;
            int result = (int)(GetRelevance(day - gossip.Created).EffectMultiplier * effect);
            return result > 1 ? result : 1;
        }

        public void Reset()
        {
            Quests.Clear();
            Gossip.Clear();
            GossipActivity=0;
        }
    }

    [Serializable]
    public struct Relevance
    {
        [Tooltip("Maximum gossip age for this relevance level")]
        [Range(1, 100)]
        public int Age;
        [Tooltip("Price modifier for this relevance level")]
        [Range(0.0f, 1.0f)]
        public float PriceMultiplier;
        [Tooltip("Effect modifier for this relevance level")]
        [Range(0.0f, 1.0f)]
        public float EffectMultiplier;
    }

    [Serializable]
    public struct GossipTier
    {
        [Tooltip("Allegiance effect of this tier")]
        [Range(1, 100)]
        public int Allegiance;
        [Tooltip("Power effect of this tier")]
        [Range(1, 100)]
        public int Power;
        [Tooltip("Base price of this tier of gossip")]
        [Range(1, 500)]
        public int Price;
    }

    [Serializable]
    public struct RewardTier
    {
        public CommodityVO[] Rewards;
    }
}
