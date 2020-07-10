using System;
using UnityEngine;

namespace Ambition
{
    // I went back and forth on defining enums for tier and relevance here,
    // but everything outside this class wants them as ints, pretty much, soooooo

    public static class GossipWrapperVO
    {
        public static FactionType GetFaction(ItemVO gossip)
        {
            if (Enum.TryParse(gossip.ID, ignoreCase:true, out FactionType factionID))
            {
                return factionID;
            }

            Debug.LogWarningFormat("GossipWrapperVO.GetFaction: couldn't parse gossip item's faction '{0}'", gossip.ID);
            return FactionType.Neutral;
        }

        public static int GetTier(ItemVO gossip) => gossip.Price;
        // up to date with 10/20 version of spec
        public static int GetRelevance(ItemVO gossip)
        {
            var _cfg = AmbitionApp.GetModel<InventoryModel>().Gossip;
            DateTime today = AmbitionApp.GetModel<GameModel>().Date;
            int result = (today - gossip.Created).Days;
            if (result > _cfg.Old.DaysThreshold) return 0;
            if (result > _cfg.Relevant.DaysThreshold) return 1;
            if (result > _cfg.Fresh.DaysThreshold) return 2;
            return 3;                   
        }

        public static int GetRelevanceSliderValue(ItemVO gossip)
        {
            var _cfg = AmbitionApp.GetModel<InventoryModel>().Gossip;
            DateTime today = AmbitionApp.GetModel<GameModel>().Date;
            return 16-((today - gossip.Created).Days);                 
        }

        public static GossipRelevanceVariableSet GetRelevanceConfig(ItemVO gossip)
        {
            var _cfg = AmbitionApp.GetModel<InventoryModel>().Gossip;

            switch (GetRelevance(gossip))
            {
                case 0: return _cfg.Irrelevant;
                case 1: return _cfg.Old;
                case 2: return _cfg.Relevant;
                case 3: return _cfg.Fresh;
            }
            Debug.LogErrorFormat("Gossip item with invalid relevance {0}", ToString(gossip) );
            return _cfg.Fresh;
        }

        public static GossipTierVariableSet GetTierConfig(ItemVO gossip)
        {
            var _cfg = AmbitionApp.GetModel<InventoryModel>().Gossip;

            switch (GetTier(gossip))
            {
                case 0: return _cfg.Cheap;
                case 1: return _cfg.Shocking;
                case 2: return _cfg.Outrageous;
            }

            Debug.LogErrorFormat("Gossip item with invalid tier {0}", ToString(gossip) );
            return _cfg.Cheap;
        }
        // up to date with 10/20 version of spec
        public static int GetValue(ItemVO gossip)
        {
            int value = (int)(GetTierConfig(gossip).PriceBase * GetRelevanceConfig(gossip).PriceMod);

            // Flat minimum for stale gossip
            if (value < 1)
            {
                value = 1;
            }

            return value;
        }
        // up to date with 10/20 version of spec
        public static int GetEffect(ItemVO gossip)
        {
            int effect = (int)(GetTierConfig(gossip).EffectBase * GetRelevanceConfig(gossip).EffectMod);

            // Flat minimum for stale gossip
            if (effect < 1)
            {
                effect = 1;
            }

            return effect;
        }
        
        public static string GetDescription(ItemVO gossip)
        {
            string shift = "???";
            if (!gossip.State.TryGetValue(ItemConsts.SHIFT, out shift))
            {
                Debug.LogErrorFormat("Gossip item didn't have a shift state variable? {0}", ToString(gossip) );
            }
            
            var locKey = string.Format( "gossip.{0}.{1}.{2}", GetTier(gossip), gossip.ID.ToLower(), shift );
            return AmbitionApp.Localize(locKey);

            //switch (gossip.State[ItemConsts.SHIFT])
            //{
            //    case ItemConsts.POWER:
            //        return "Depending on how we spin this, this could improve or damage the standings of the " + gossip.ID + " by eleventy-one";
            //}
            //return "With some creative prose, this could be used to push the " + gossip.ID + " towards either the Crown or the Revolution by eleventy-one";
        }

        public static bool IsPowerShift( ItemVO gossip )
        {
            return gossip.State[ItemConsts.SHIFT] == ItemConsts.POWER;
        } 

        public static string ToString( ItemVO gossip )
        {
            string shift = "???";
            if (!gossip.State.TryGetValue(ItemConsts.SHIFT, out shift))
            {
                Debug.LogError("Gossip item didn't have a shift state variable?" );
            }
            return string.Format( "ItemVO {0} '{1}'", gossip.Name, GetDescription(gossip) );

        }
    }
}
