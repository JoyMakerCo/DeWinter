using System;
using UnityEngine;

namespace Ambition
{
    public class TimerTutorialController : MonoBehaviour
    {
        void Awake()
        {
            if (!AmbitionApp.IsActiveState("TutorialController"))
                Destroy(this);
            else AmbitionApp.Subscribe(PartyMessages.SHOW_ROOM, HandleTutorialRoom);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, HandleTutorialRoom);
        }
   
        private void HandleTutorialRoom()
        {
            TurnTimerView tv = GetComponent<TurnTimerView>();
            if (tv != null) tv.enabled = !tv.enabled;
            if (tv.enabled) Destroy(this);
        }
    }
}
