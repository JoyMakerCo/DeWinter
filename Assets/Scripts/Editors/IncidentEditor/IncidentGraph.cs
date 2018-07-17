using Util;

namespace Ambition
{
    public class IncidentGraph : DirectedGraph<MomentVO, TransitionVO>
    {
        public string Name;
		public IncidentSetting Setting;
    }
}
