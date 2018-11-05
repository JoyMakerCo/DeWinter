using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RoomTitleScript : MonoBehaviour
    {
        private Text _titleText;
        public Toggle[] StarToggles;

		void Awake ()
		{
			_titleText = gameObject.GetComponent<Text>();
            HandleRoom(AmbitionApp.GetModel<MapModel>().Room);
		}

		private void HandleRoom(RoomVO room)
		{
            _titleText.text = room.Name;
            HandleStarToggles(room.Difficulty);
		}

        private void HandleStarToggles(int stars)
        {
            for (int i = 0; i < StarToggles.Length; i++)
            {
                if(i+1 <= stars)
                {
                    StarToggles[i].isOn = true;
                } else
                {
                    StarToggles[i].isOn = false;
                }
            }
        }
	}
}
