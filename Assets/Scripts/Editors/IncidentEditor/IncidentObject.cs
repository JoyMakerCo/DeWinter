using Util;

namespace Ambition
{
    public class IncidentObject : DirectedGraphObject<MomentVO, TransitionVO>
    {
        void Marshall(IncidentVO incident)
        {
            Graph = new IncidentGraph();
        }
    }
}
