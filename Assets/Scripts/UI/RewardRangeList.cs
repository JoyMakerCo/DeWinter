using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class RewardRangeList : SortableList<RewardRange>
    {
        public GameObject ListContainer;

        private IncidentVO _incident = null;
        private List<RewardRange> _rewards = null;

        // Incident must be set for the list to populate
        public void SetIncident(IncidentVO incident)
        {
            _incident = incident;
            if (_incident?.Nodes != null)
            {
                _rewards = TraceRewards(0, new List<RewardRange>(), new List<int>());
            }
            if (ListContainer != null) ListContainer.SetActive(_rewards != null && _rewards.Count > 0);
        }

        // Required override. Returns the traced list of reward ranges.
        protected override RewardRange[] FetchListData() => _rewards?.ToArray() ?? new RewardRange[0];

        // Recursive call to nodes in order to trace their values.
        private List<RewardRange> TraceRewards(int node, List<RewardRange> rewards, List<int> visited)
        {
            if (visited.Contains(node)) return null;
            int[] neighbors = _incident.GetNeighbors(node);
            if (neighbors.Length == 0) return rewards;
            List<int> vs = new List<int>(visited);
            List<RewardRange> nodeRewards = Accumulate(rewards, _incident.Nodes[node].Rewards);
            List<RewardRange> result = new List<RewardRange>();
            List<RewardRange> temp = new List<RewardRange>();
            vs.Add(node);
            foreach(int n in neighbors)
            {
                temp = TraceRewards(n, nodeRewards, vs);
                if (temp != null)
                    result = Accumulate(result, temp);
            }
            return result;
        }

        // Adds the contents from a list of rewards to the list of ranges
        private List<RewardRange> Accumulate(List<RewardRange> ranges, CommodityVO[] rewards)
        {
            List<RewardRange> result = new List<RewardRange>(ranges);
            List<string> gossip = new List<string>();
            GossipModel model = AmbitionApp.GetModel<GossipModel>();
            string gossipName;
            RewardRange range;
            CharacterVO character;
            foreach (CommodityVO reward in rewards)
            {
                switch (reward.Type)
                {
                    case CommodityType.Gossip:
                        gossipName = model.GetName(reward);
                        if (!gossip.Contains(gossipName))
                        {
                            gossip.Add(gossipName);
                            range = new RewardRange(CommodityType.Gossip, reward.ID, reward.Value, reward.Value);
                            result.Add(range);
                        }
                        break;
                    case CommodityType.Favor:
                        character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(reward.ID);
                        if (character?.FavoredLocations?.Length + character?.OpposedLocations?.Length > 0)
                        {
                            UpdateOrAdd(ref result, reward.Type, reward.ID, reward.Value, reward.Value);
                        }
                        break;
                    case CommodityType.Livre:
                    case CommodityType.Credibility:
                    case CommodityType.Peril:
                    case CommodityType.Servant:
                        UpdateOrAdd(ref result, reward.Type, reward.ID, reward.Value, reward.Value);
                        break;
                }
            }
            return result;
        }

        // Adds new ranges or expands existing ones
        private List<RewardRange> Accumulate(List<RewardRange> ranges, List<RewardRange> rewards)
        {
            List<RewardRange> result = new List<RewardRange>(ranges);
            List<string> gossip = new List<string>();
            GossipModel model = AmbitionApp.GetModel<GossipModel>();
            string gossipName;
            RewardRange range;
            CharacterVO character;
            CommodityVO commodity;
            foreach (RewardRange reward in rewards)
            {
                switch (reward.Type)
                {
                    case CommodityType.Gossip:
                        commodity = new CommodityVO(CommodityType.Gossip, reward.ID, reward.Low);
                        gossipName = model.GetName(commodity);
                        if (!gossip.Contains(gossipName))
                        {
                            gossip.Add(gossipName);
                            range = new RewardRange(ref commodity);
                            result.Add(range);
                        }
                        break;
                    case CommodityType.Favor:
                        character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(reward.ID);
                        if (character?.FavoredLocations?.Length + character?.OpposedLocations?.Length > 0)
                        {
                            UpdateOrAdd(ref result, reward.Type, reward.ID, reward.Low, reward.High);
                        }
                        break;
                    case CommodityType.Livre:
                    case CommodityType.Credibility:
                    case CommodityType.Peril:
                    case CommodityType.Servant:
                        UpdateOrAdd(ref result, reward.Type, reward.ID, reward.Low, reward.High, true);
                        break;
                }
            }
            return result;
        }

        // Examines a list to see if a type of reward exists, and adds one if none exists
        private void UpdateOrAdd(ref List<RewardRange> ranges, CommodityType type, string id, int low, int high, bool addZeroToRange=false)
        {
            int index = string.IsNullOrEmpty(id)
                ? ranges.FindIndex(r => r.Type == type && string.IsNullOrEmpty(r.ID))
                : ranges.FindIndex(r => r.Type == type && r.ID == id);
            if (index < 0)
            {
                if (0 < low) low = 0;
                else if (0 > high) high = 0;
                ranges.Add(new RewardRange(type, id, low, high));
            }
            else
            {
                RewardRange range = ranges[index];
                if (low < range.Low) range.Low = low;
                if (high > range.High) range.High = high;
                ranges[index] = range;
            }
        }
    }
}
