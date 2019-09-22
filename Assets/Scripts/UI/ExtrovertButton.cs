using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Ambition
{
    public class ButtonEventArgs : EventArgs
    {
        public int SelectionState;
        public bool Instant;
        public ButtonEventArgs(int state, bool instant) : base()
        {
            SelectionState = state;
            Instant = instant;
        }
    }

    public class ExtrovertButton : Button
    {
        public event EventHandler<ButtonEventArgs> ButtonEvent;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            ButtonEvent?.Invoke(this, new ButtonEventArgs((int)state, instant));
        }
    }
}
