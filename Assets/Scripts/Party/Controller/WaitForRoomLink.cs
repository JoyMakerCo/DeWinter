using UFlow;
namespace Ambition
{
    public class WaitForRoomLink : ULink
    {
        public override void Initialize() => AmbitionApp.Subscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        public override void Dispose() => AmbitionApp.Unsubscribe<RoomVO>(MapMessage.GO_TO_ROOM, HandleRoom);
        private void HandleRoom(RoomVO room) => Activate();
    }
}
