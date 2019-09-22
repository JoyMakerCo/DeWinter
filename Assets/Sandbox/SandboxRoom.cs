using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class SandboxRoom : MonoBehaviour
    {
        private const int WIDTH = 100;

        private List<GuestMediator> _guests = new List<GuestMediator>();

        public void AddGuest(GuestMediator guest)
        {
            _guests.Add(guest);
            int total = WIDTH * _guests.Count;
            for(int i=0; i<_guests.Count; i++)
            {
                //guest.GoRoom();
            }
        }

        public void Visit()
        {
            _guests.ForEach(g=>Visit());
        }
    }
}
