using System;
using UFlow;

namespace Ambition
{
    public class CheckEndTutorialLink : ULink
    {
        public override void Initialize()
        {
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, CheckMark);
        }

        private void CheckMark()
        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            if (map.Map != null
                && map.Map.Rooms != null
                && Array.Exists(map.Map.Rooms, r=>r.HostHere && r.Cleared))
            {
                Activate();
            }
        }

        public override void Dispose()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, CheckMark);
        }
    }
}
