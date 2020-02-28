using System;
using UnityEngine;
namespace Ambition
{
    public class PickMapState : UFlow.UState
    {
        public override void OnEnterState()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            PartyVO party = model.Party;
            if (party == null)
                throw new Exception(">> Ambition Error: No Party Selected!");
            MapModel mapModel = AmbitionApp.GetModel<MapModel>();
            if (!mapModel.LoadMap(party))
            {
                MapVO[] maps = Array.FindAll(mapModel.Maps,
                    m => m.Faction == party.Faction || m.Faction == FactionType.Neutral
                );

                // TODO: Figure out significance of tagging scheme
                //MapModel.MapConfig[] tagConfigs = Array.FindAll(configs, c=>(
                //party.Tags == null || party.Tags.Length == 0
                //Array.TrueForAll(party.Tags, t => Array.IndexOf(c.Tags, t) >= 0)));
                //party.MapID = Util.RNG.TakeRandom(tagConfigs.Length == 0 ? configs : tagConfigs).Name;
                mapModel.Map.Value = Util.RNG.TakeRandom(maps);
            }
        }
    }
}
