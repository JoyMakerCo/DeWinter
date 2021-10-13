using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
    public class GossipConfig : ScriptableObject, IModelConfig
    {
		[Header("Gossip Relevance Levels")]
        public Relevance[] Relevance;

        [Header("Gossip Effect Tiers")]
        public GossipTier[] Tiers;

        [Header("Reproach Chance for Gossip Trade")]
        public int[] ReproachChance;

        [Header("Reward Values by Tier")]
        public RewardTier[] RewardTiers;

        public int MaxQuests = 1;

        public void Register(Core.ModelSvc modelService)
        {
            GossipModel model = modelService.Register<GossipModel>();
            Array.Sort(Relevance, (a, b) => a.Age.CompareTo(b.Age));
            model.Relevances = Relevance;
            model.Tiers = Tiers;
            model.MaxQuests = MaxQuests;
            model.ReproachChance = ReproachChance;
            model.RewardTiers = RewardTiers;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/GossipConfig")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<GossipConfig>("Gossip Config");
#endif
    }
}
