using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class DailyPlacardView : MonoBehaviour
    {
        public FMODEvent AudioSting;

        private void Start()
        {
            Text text = gameObject.GetComponentInChildren<Text>();
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            AmbitionApp.SendMessage(GameMessages.HIDE_HEADER);
            AmbitionApp.SendMessage(AudioMessages.STOP_AMBIENT);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            if (!string.IsNullOrWhiteSpace(AudioSting.Name))
                AmbitionApp.SendMessage(AudioMessages.PLAY, AudioSting);
            if (text != null) text.text = AmbitionApp.GetModel<LocalizationModel>().Date;
            AmbitionApp.SendMessage(GameMessages.INHIBIT_MENU);
        }
    }
}
