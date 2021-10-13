using System;
using UnityEngine;
namespace Ambition
{
    public class FactionConfig : ScriptableObject, IModelConfig
    {
        public FactionVO[] Factions;

        public void Register(Core.ModelSvc modelSvc)
        {
            FactionModel model = modelSvc.Register<FactionModel>();
            FactionStandingsVO standings;
            model.Factions.Clear();
            model.ResetValues.Clear();
            foreach (FactionVO faction in Factions)
            {
                model.Factions[faction.Type] = new FactionVO(faction);
                standings = new FactionStandingsVO()
                {
                    Faction = faction.Type,
                    Power = faction.Power,
                    Allegiance = faction.Allegiance
                };
                model.ResetValues.Add(standings);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/FactionModel")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<FactionConfig>("Faction Model");
#endif
    }
}
