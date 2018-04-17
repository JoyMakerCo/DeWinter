using UnityEngine;

namespace Ambition
{
    public class LeavePartyBtn : MonoBehaviour
    {
        private bool _subscribed=false;
        void OnEnable()
        {
            if (!AmbitionApp.IsActiveState(TutorialConsts.TUTORIAL)) Destroy(this);
            else if (!_subscribed)
            {
                AmbitionApp.Subscribe(PartyMessages.SHOW_MAP, HandleShowMap);
                gameObject.SetActive(false);
                _subscribed = true;
            }
        }
        
        void OnDestroy()
        {
            gameObject.SetActive(true);
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, HandleShowMap);
        }

        private void HandleShowMap()
        {
            RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
            if (room.HostHere) Destroy(this);
        }
    }
}
