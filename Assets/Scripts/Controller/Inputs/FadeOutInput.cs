using System;
namespace Ambition
{
    public class FadeOutInput : UFlow.UInput, IDisposable
    {
        public FadeOutInput() => AmbitionApp.Subscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
        public override void OnEnter() => AmbitionApp.SendMessage(GameMessages.FADE_OUT);
        public void Dispose() => AmbitionApp.Unsubscribe(GameMessages.FADE_OUT_COMPLETE, Activate);
    }
}
