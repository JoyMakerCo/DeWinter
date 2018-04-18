using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFlow;

namespace Ambition
{
    public class TutorialGuestState : TutorialState
    {
        private TutorialFlashSpot _flash=null;
        override public void OnEnterState()
        {
            base.OnEnterState();
            GameObject canvas = Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), o=>o.GetComponent<Dialog.DialogCanvasManager>() != null);
            if (canvas != null) 
            {
                RemarkVO rem = AmbitionApp.GetModel<PartyModel>().Remark;
                GuestVO[] guests = AmbitionApp.GetModel<MapModel>().Room.Guests;
                SpotlightView[] spotlights = canvas.GetComponentsInChildren<SpotlightView>(true); //Changed from false because the spotlights start as disabled and this array was coming up empty - Luther
                int index = Array.FindIndex(guests, g=>g.Like == rem.Interest);
                if (index < 0) index = Array.FindIndex(guests, g=>g.Dislike != rem.Interest);
                if (index < 0) index = 0;
                GameObject obj = Array.Find(spotlights, s=>s.Guest.Guest == guests[index]).gameObject;
                _flash = obj.AddComponent<TutorialFlashSpot>();
            }
        }

        override public void OnExitState()
        {
            if (_flash != null) GameObject.Destroy(_flash);
            base.OnExitState();
        }
    }
}
