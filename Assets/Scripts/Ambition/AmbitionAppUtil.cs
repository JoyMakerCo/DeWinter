using System;
using System.Collections.Generic;
using UnityEngine;
namespace Ambition
{
    public static partial class AmbitionApp
    {
        public static List<CommodityVO> GetRewards(IncidentVO incident)
        {
            return incident != null
                ? TraceRewards(incident, 0, new List<CommodityVO>(), new List<int>())
                : new List<CommodityVO>();
        }

        public static bool CheckIncidentEligible(string incidentID) => CheckIncidentEligible(AmbitionApp.Story.GetIncident(incidentID));
        public static bool CheckIncidentEligible(IncidentVO incident)
        {
            if (AmbitionApp.Story.IsComplete(incident?.ID, true))
                return false;
            if (!AmbitionApp.CheckRequirements(incident.Requirements))
                 return false;
            if (incident.Chapters?.Length == 0)
                return true;
            if (Array.IndexOf(incident.Chapters, AmbitionApp.Game.Chapter) >= 0)
                return true;
            return false;
            //return !AmbitionApp.Story.IsComplete(incident?.ID, true)
            //&& AmbitionApp.CheckRequirements(incident.Requirements)
            //&& (incident.Chapters?.Length == 0 || Array.IndexOf(incident.Chapters, AmbitionApp.Game.Chapter) >= 0);
        }

        private static List<CommodityVO> TraceRewards (IncidentVO incident, int node, List<CommodityVO> rewards, List<int> traversed)
        {
            if (traversed.Contains(node) || node >= (incident?.Nodes?.Length ?? 0)) return rewards; // Prevent circular references

            MomentVO moment = incident.Nodes[node];
            CommodityVO item;
            traversed.Add(node);
            if (moment.Rewards != null)
            {
                foreach (CommodityVO reward in moment.Rewards)
                {
                    switch (reward.Type)
                    {
                        case CommodityType.Gossip:
                        case CommodityType.Item:
                            rewards.Add(reward.Clone());
                            break;
                        case CommodityType.Livre:
                        case CommodityType.Peril:
                        case CommodityType.Credibility:
                            item = rewards.Find(r => r.Type == reward.Type);
                            if (item == null) rewards.Add(reward.Clone());
                            else item.Value += reward.Value;
                            break;
                        case CommodityType.Favor:
                            item = rewards.Find(r => r.ID == reward.ID);
                            if (item == null) rewards.Add(reward.Clone());
                            else item.Value += reward.Value;
                            break;
                    }
                }
            }
            int[] neighbors = incident.GetNeighbors(node);
            if (neighbors.Length == 0) return rewards;


            foreach (int neighbor in neighbors)
            {
                rewards = TraceRewards(incident, neighbor, rewards, new List<int>(traversed));
            }
            return rewards;
        }

        public static List<CommodityVO> TallyRewards(IEnumerable<CommodityVO> rewards)
        {
            List<CommodityVO> result = new List<CommodityVO>();
            Dictionary<string, CommodityVO> tallies = new Dictionary<string, CommodityVO>();
            string key;
            CommodityVO tally;
            foreach(CommodityVO value in rewards)
            {
                key = null;
                switch(value.Type)
                {
                    case CommodityType.Gossip:
                        if (!result.Exists(g=>g.Type == CommodityType.Gossip && g.ID == value.ID && g.Value == value.Value))
                            result.Add(value);
                        break;
                    case CommodityType.Item:
                        result.Add(value);
                        break;
                    case CommodityType.Favor:
                        key = value.ID;
                        break;
                    default:
                        key = value.Type.ToString();
                        break;
                }
                if (key != null)
                {
                    tallies.TryGetValue(key, out tally);
                    if (tally == null) tallies[key] = value.Clone();
                    else tallies[key].Value += value.Value;
                }
            }
            result.AddRange(tallies.Values);
            return result;
        }

        public static List<RewardItem> CreateRewardListItems(IEnumerable<CommodityVO> commodities, RewardItem listItem)
        {
            List<RewardItem> result = new List<RewardItem>();
            List<CommodityVO> rewards = TallyRewards(commodities);
            GossipModel model = AmbitionApp.GetModel<GossipModel>();
            RewardItem item = listItem;
            List<string> duplicates = new List<string>();
            string gossipName;
            GameObject obj;
            bool doCreate;
            CharacterModel characterModel = AmbitionApp.GetModel<CharacterModel>();
            CharacterVO character;
            foreach (CommodityVO reward in rewards)
            {
                switch (reward.Type)
                {
                    case CommodityType.Gossip:
                        gossipName = model.GetName(reward);
                        doCreate = true;
                        break;
                    case CommodityType.Favor:
                        character = characterModel.GetCharacter(reward.ID);
                        doCreate = character != null
                            && character.FavoredLocations?.Length + character.OpposedLocations?.Length > 0
                            && !duplicates.Contains(reward.ID);
                        if (!doCreate) duplicates.Add(reward.ID);
                        break;
                    case CommodityType.Livre:
                    case CommodityType.Credibility:
                    case CommodityType.Peril:
                        doCreate = true;
                        break;
                    default:
                        doCreate = false;
                        break;
                }
                if (doCreate)
                {
                    if (item == null)
                    {
                        obj = GameObject.Instantiate<GameObject>(listItem.gameObject, listItem.transform.parent);
                        item = obj.GetComponent<RewardItem>();
                    }
                    item.Data = reward;
                    item.gameObject.SetActive(true);
                    result.Add(item);
                    item = null;
                }
            }
            listItem.gameObject.SetActive(result.Count > 0);
            return result;
        }

        public static CalendarEvent GetEvent() => GetEvent(Calendar.Day);
        public static CalendarEvent GetEvent(int day)
        {
            PartyVO[] parties = AmbitionApp.Calendar.GetOccasions<PartyVO>(day);
            PartyVO party = Array.Find(parties, p => p.IsAttending);
            if (party != null) return party;

            RendezVO[] rendezs = Calendar.GetOccasions<RendezVO>(day);
            return Array.Find(rendezs, r => r.IsAttending);
        }

        public static CalendarEvent[] GetEvents() => GetEvents(Calendar.Day);
        public static CalendarEvent[] GetEvents(int day)
        {
            List<CalendarEvent> result = new List<CalendarEvent>();
            result.AddRange(Calendar.GetOccasions<PartyVO>(day));
            result.AddRange(Calendar.GetOccasions<RendezVO>(day));
            return result.ToArray();
        }

        public static CalendarEvent GetNextEvent(int days)
        {
            CalendarEvent[] events;
            CalendarEvent e;
            int day;
            for (int i = 1; i < days; ++i)
            {
                day = Calendar.Day + i;
                events = Calendar.GetOccasions<PartyVO>(day);
                e = Array.Find(events, r => r.IsAttending);
                if (e != null) return e;
                events = Calendar.GetOccasions<RendezVO>(day);
                e = Array.Find(events, r => r.IsAttending);
                if (e != null) return e;
            }
            return null;
        }
    }
}
