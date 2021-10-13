using System;
using UnityEngine;
namespace Ambition
{
    public class CharacterModelConfig : UnityEngine.ScriptableObject, IModelConfig
    {
        public int LiaisonChance = 100;
        public int MissedRendezvousPenalty = -2;
        public void Register(Core.ModelSvc svc)
        {
            CharacterModel model = svc.Register<CharacterModel>();
            model.LiaisonChance = LiaisonChance;
            model.MissedRendezvousPenalty = MissedRendezvousPenalty;
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/CharacterModelConfig")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<CharacterModelConfig>("CharacterModel");
#endif
    }
}
