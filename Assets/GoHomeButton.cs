using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class GoHomeButton : MonoBehaviour
    {
        public void GoHome()
        {
            AmbitionApp.SendMessage(CalendarMessages.NEXT_DAY);
        }
    }
}

