﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UFlow;

namespace Ambition
{
    public class TutorialGuestState : TutorialState, IDisposable
    {
        private TutorialFlashSpot _flash=null;
        public override void OnEnterState()
        {
            if (_flash != null) GameObject.Destroy(_flash);
            GameObject canvas = Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), o=>o.GetComponent<Canvas>() != null);
            if (canvas != null)
            {
                ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
                RemarkVO rem = model.Remark;
                if (rem != null)
                {
                    //CharacterVO[] guests = model.Guests;
                    SpotlightView[] spotlights = canvas.GetComponentsInChildren<SpotlightView>(true);
                    //int index = Array.FindIndex(guests, g => g.Like == rem.Interest);
                    //if (index < 0) index = Array.FindIndex(guests, g => g.Dislike != rem.Interest);
                    //if (index < 0) index = 0;
                    //_flash = spotlights[index].gameObject.AddComponent<TutorialFlashSpot>();
                }
            }
        }

        public override void Dispose()
        {
            if (_flash != null) GameObject.Destroy(_flash);
            base.Dispose();
        }
    }
}
