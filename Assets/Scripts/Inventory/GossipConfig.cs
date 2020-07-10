using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ambition
{
	[Serializable]
	public class GossipRelevanceVariableSet
	{
		[Tooltip("Maximum gossip age for this relevance level")]
		[Range(1,60)]
		public int DaysThreshold = 5;

		[Tooltip("Price modifier for this relevance level")]
		[Range(0.0f,1.0f)]
        public float PriceMod = 1.0f;

		[Tooltip("Effect modifier for this relevance level")]
		[Range(0.0f,1.0f)]
        public float EffectMod = 1.0f;

	}
	[Serializable]
	public class GossipTierVariableSet
	{
		[Tooltip("Effect level of this tier of gossip")]
		[Range(1,24)]
		public int EffectBase = 2;
		[Tooltip("Base price of this tier of gossip")]
		[Range(1,400)]
		public int PriceBase = 30;
	}

    public class GossipConfig : ScriptableObject
    {
		[Header("Gossip Relevance Levels")]
		public GossipRelevanceVariableSet Fresh;
		public GossipRelevanceVariableSet Relevant;
		public GossipRelevanceVariableSet Old;
		public GossipRelevanceVariableSet Irrelevant;

		[Header("Gossip Effect Tiers")]
		public GossipTierVariableSet Cheap;
		public GossipTierVariableSet Shocking;
		public GossipTierVariableSet Outrageous;

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/GossipConfig")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<GossipConfig>("Gossip Config");
#endif
    }
}
