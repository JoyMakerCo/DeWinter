using System;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class Dimmer : MonoBehaviour
    {
        public UnityEngine.UI.Image Blocker;
        public DialogManager Dialogs;

        private void Awake()
        {
            Dialogs.OnOpenDialog += DimEvent;
            Dialogs.OnCloseDialog += DimEvent;
        }

        private void DimEvent(object sender, DialogEventArgs a)
        {
            Color color = Blocker.color;
            color.a = Dialogs.NumDialogs > 0 ? .7f : 0f;
            Blocker.color = color;
        }
    }
}
