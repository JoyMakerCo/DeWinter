﻿using System;
using UnityEngine;

namespace Ambition
{
    public class GuestActionView : GuestViewMediator
    {
        public ActionMap[] Actions;

        [Serializable]
        public struct ActionMap
        {
            public string ActionType;
            public GameObject View;
        }

        private void Awake() => InitGuest();
        private void OnDestroy() => Cleanup();
        private void Start() => gameObject.SetActive(false);

        override protected void HandleGuest(CharacterVO guest)
        {
            if (guest != null && guest == _guest)
            {
                bool activate = false; //guest.Action != null;
                gameObject.SetActive(activate);
                if (activate)
                {
                    //GuestActionVO action = guest.Action;
                    //foreach (ActionMap map in Actions)
                    //{
                    //    activate = map.ActionType == action.Type;
                    //    map.View.SetActive(activate);
                    //    if (activate)
                    //    {
                    //        icon = map.View.GetComponent<GuestActionIcon>();
                    //        if (icon != null) icon.SetAction(action);
                    //    }
                    //}
                }
            }
        }
    }
}
