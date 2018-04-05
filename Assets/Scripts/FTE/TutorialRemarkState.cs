using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFlow;

namespace Ambition
{
    public class TutorialRemarkState : TutorialState
    {
        private TutorialBounce _bounz=null;
        override public void OnEnterState()
        {
            base.OnEnterState();
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            MapModel map = AmbitionApp.GetModel<MapModel>();
            int index = model.Remarks.FindIndex(r=>Array.Exists(map.Room.Guests, g=>g.Like == r.Interest));
            if (index < 0) model.Remarks.FindIndex(r=>Array.Exists(map.Room.Guests, g=>g.Dislike != r.Interest));
            if (index < 0) index = 0;

            GameObject canvas = Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), o=>o.GetComponent<Dialog.DialogCanvasManager>() != null);
            if (canvas != null) 
            {
                RemarkView[] remarks = canvas.GetComponentsInChildren<RemarkView>(false);
                GameObject obj = Array.Find(remarks, r=>r.transform.GetSiblingIndex() == index).gameObject;
                _bounz = obj.AddComponent<TutorialBounce>();
            }
        }

        override public void OnExitState()
        {
            if (_bounz != null) GameObject.Destroy(_bounz);
            base.OnExitState();
        }
    }
}
