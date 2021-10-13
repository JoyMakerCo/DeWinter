using System;
using UnityEngine;

namespace Ambition
{
    public class GameModelConfig : ScriptableObject, IModelConfig
    {
        public int PoliticalChance;
        public int MissedPartyPenalty;
        public int WellRestedBonus;
        public int[] ExhuastionPenalty;
        public int MaxSaves;

        public void Register(Core.ModelSvc modelService)
        {
            GameModel model = modelService.Register<GameModel>();
            model.PoliticalChance = PoliticalChance;
            model.MissedPartyPenalty = MissedPartyPenalty;
            model.WellRestedBonus = WellRestedBonus;
            model.ExhaustionPenalties = ExhuastionPenalty;
            model.MaxSaves = MaxSaves;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/GameModel")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<GameModelConfig>("Game Model");
#endif
    }
}
