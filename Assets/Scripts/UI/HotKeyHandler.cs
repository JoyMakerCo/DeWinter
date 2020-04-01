using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

namespace Ambition
{
	// mostly for dev/debug purposes
    public class HotKeyHandler : MonoBehaviour
    {
        private bool _inhibited;
        private void Awake()
        {
            _inhibited = false;
        }
        void Update()
        {
			if (_inhibited)
			{
				return;
			}
            if (Input.GetKeyDown(KeyCode.F10))
            {
            	AmbitionApp.SendMessage(GameMessages.TOGGLE_CONSOLE);
            }
        }

		void inhibit( bool state )
		{
			_inhibited = state;
		}


    }
}
