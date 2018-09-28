using UFlow;
namespace Ambition
{
    public class WaitForRoomLink : ULink
    {
        public override void Initialize() => AmbitionApp.Subscribe<RoomVO>(HandleRoom);
        private void HandleRoom(RoomVO room) => Activate();
        public override void Dispose() => AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
    }
}
