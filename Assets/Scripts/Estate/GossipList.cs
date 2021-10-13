using System;
namespace Ambition
{
    public class GossipList : SortableList<GossipVO>
    {
        public override void Initialize() => AmbitionApp.Gossip.Observe(HandleRefresh);
        public override void Dispose() => AmbitionApp.Gossip.Unobserve(HandleRefresh);
        private void HandleRefresh(GossipModel model) => Refresh();
        protected override GossipVO[] FetchListData() => AmbitionApp.Gossip.Gossip.ToArray();

        protected override Comparison<GossipVO> GetComparer(int sortIndex)
        {
            switch(sortIndex)
            {
                case 1: return SortByFaction;
                case 2: return SortByFreshness;
            }
            return SortByValue;
        }

        private int SortByValue(GossipVO x, GossipVO y) => AmbitionApp.Gossip.GetValue(x, AmbitionApp.Calendar.Day).CompareTo(AmbitionApp.Gossip.GetValue(y, AmbitionApp.Calendar.Day));
        private int SortByFaction(GossipVO x, GossipVO y) => x.Faction.CompareTo(y.Faction);
        private int SortByFreshness(GossipVO x, GossipVO y) => x.Created.CompareTo(y.Created);
    }
}
