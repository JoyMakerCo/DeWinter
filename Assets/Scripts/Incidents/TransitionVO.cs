using System;
namespace Ambition
{
    [Serializable]
    public class TransitionVO
    {
        public string Text;
        public bool xor = false;
        public CommodityVO[] Rewards;
        public RequirementVO[] Requirements;
        public IncidentFlag[] Flags;

        public override string ToString()
        {
            return string.Format("TransitionVO: {0} {1} rewards, {2} requirements", Text.Truncate(16), Rewards.Length, Requirements.Length);
        }
    }
}
