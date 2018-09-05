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
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            GameObject canvas = Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), o=>o.GetComponent<Dialog.DialogManager>() != null);
            if (canvas != null) 
            {
                RemarkView[] remarks = canvas.GetComponentsInChildren<RemarkView>(false);
                int index = model.Remarks.FindIndex(r => Array.Exists(model.Guests, g => g.Like == r.Interest));
                if (index < 0) model.Remarks.FindIndex(r => Array.Exists(model.Guests, g => g.Dislike != r.Interest));
                if (index < 0) index = 0;

                GameObject obj = Array.Find(remarks, r=>r.transform.GetSiblingIndex() == index).gameObject;
                _bounz = obj.AddComponent<TutorialBounce>();
            }
        }

        override public void Dispose()
        {
            GameObject.Destroy(_bounz);
        }
    }
}
