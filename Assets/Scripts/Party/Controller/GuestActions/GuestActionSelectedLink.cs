using UFlow;

namespace Ambition
{
    public class GuestActionSelectedLink:ULink<string>
    {
        private string _actionType;
        override public void SetValue(string data) => _actionType = data;
        override public bool Validate() => false;
/*        {
            MapModel map = AmbitionApp.GetModel<MapModel>();
            UController controller = _machine._UFlow.GetController(_machine);
            int index = controller.transform.GetSiblingIndex();
            if (index >= map.Room.Value.Guests.Length) return false;
            CharacterVO guest = map.Room.Value.Guests[index];
            return (guest.Action != null && guest.Action.Type == _actionType);
        }
*/    }
}
