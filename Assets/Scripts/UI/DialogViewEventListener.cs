using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ambition
{
    public class DialogViewEventListener : Dialog.DialogView
    {
        public UnityEvent OnOpenEvent;
        public UnityEvent OnCloseEvent;

        public override void OnOpen() => OnOpenEvent.Invoke();
        public override void OnClose() => OnCloseEvent.Invoke();
    }
}
