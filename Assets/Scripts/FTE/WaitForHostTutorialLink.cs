using System;
using UFlow;
namespace Ambition
{
    public class WaitForHostTutorialLink : ULink
    {
        private RoomVO _room;
        override public void Initialize()
        {
            MapModel model = AmbitionApp.GetModel<MapModel>();
            _room = Array.Find(model.Map.Rooms, r=>r.HostHere);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleMap);
            HandleMap();
        }

        override public void Dispose()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleMap);
        }

        private void HandleMap()
        {
            if (_room.Cleared) Activate();
        }
    }
}
