using System;
using UnityEngine;
namespace Ambition
{
    public class PartyModelConfig : ScriptableObject, IModelConfig
    {
        public int AcceptInvitationBonus = 5;
        public int IgnoreInvitationPenalty = -5;
        public int BaseNoveltyLoss;
        public int CumulativeNoveltyLoss;
        public int[] NumTurnsByPartySize;

        public void Register(Core.ModelSvc modelService)
        {
            modelService.Register<CalendarModel>();
            PartyModel model = modelService.Register<PartyModel>();
            model.AcceptInvitationBonus = AcceptInvitationBonus;
            model.IgnoreInvitationPenalty = IgnoreInvitationPenalty;
            model.BaseNoveltyLoss = BaseNoveltyLoss;
            model.CumulativeNoveltyLoss = CumulativeNoveltyLoss;
            model.NumTurnsByPartySize = NumTurnsByPartySize ?? new int[1] { 1 };
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/PartyModel")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<PartyModelConfig>("Party Model");
#endif
    }
}
