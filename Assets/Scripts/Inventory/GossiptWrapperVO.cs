using System;
namespace Ambition
{
    public static class GossiptWrapperVO
    {
        public static int GetTier(ItemVO gossip) => gossip.Price;
        public static int GetRelevance(ItemVO gossip)
        {
            DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
            int result = (today - gossip.Created).Days;
            if (result > 12) return 0;
            if (result > 9) return 1;
            if (result > 6) return 2;
            return 3;
        }

        public static int GetValue(ItemVO gossip) => GetRelevance(gossip) * gossip.Price * 50;
        public static int GetEffect(ItemVO gossip) => GetRelevance(gossip) * gossip.Price * 5;
        public static string GetDescription(ItemVO gossip) // TODO: localize
        {
            switch (gossip.State[ItemConsts.SHIFT])
            {
                case ItemConsts.POWER:
                    return "Depending on how we spin this, this could improve or damage the standings of the " + gossip.ID + " by eleventy-one";
            }
            return "With some creative prose, this could be used to push the " + gossip.ID + " towards either the Crown or the Revolution by eleventy-one";
        }

    }
}
