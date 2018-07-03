using System;
using UnityEngine;

namespace Ambition
{
    public abstract class GuestActionIconView : MonoBehaviour
    {
        public abstract GuestActionVO Action
        {
            set; get;
        }
    }
}
