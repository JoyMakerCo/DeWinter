using UFlow;
namespace Ambition
{
    public class WaitForHostTutorialLink : ULink, Util.IInitializable, System.IDisposable
    {
        private RoomVO _room;

        public override bool Validate() => false;

        public void Initialize()
        {
            MapModel model = AmbitionApp.GetModel<MapModel>();
            //_room = model.Room;//Array.Find(model.Map.Rooms, r=>r.HostHere);
            AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleMap);
            HandleMap();
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleMap);
        }

        private void HandleMap()
        {
            if (_room.Cleared) Activate();
        }
    }
}
