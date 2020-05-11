using UFlow;
namespace Ambition
{
    public class WaitForRoomLink : ULink, Util.IInitializable, System.IDisposable
    {
        public override bool Validate() => false;
        public void Initialize() => AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        public void Dispose() => AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        private void HandleRoom(RoomVO room) => Activate();
    }
}
