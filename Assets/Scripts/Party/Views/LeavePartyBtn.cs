using UnityEngine;

namespace Ambition
{
    public class LeavePartyBtn : MonoBehaviour
    {
        private bool _subscribed=false;
        void OnEnable()
        {
            if (!_subscribed)
            {
                AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleShowMap);
                gameObject.SetActive(false);
                _subscribed = true;
            }
        }
        
        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleShowMap);
        }

        private void HandleShowMap()
        {
            RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
            if (room.HostHere)
            {
                gameObject.SetActive(true);
                Destroy(this);
            }
        }
    }
}
