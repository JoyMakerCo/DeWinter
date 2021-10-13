using System;
namespace Ambition
{
    public class FadeInInput : UFlow.UInput, IDisposable
    {
        public FadeInInput() => AmbitionApp.Subscribe(GameMessages.FADE_IN_COMPLETE, Activate);
        public void Dispose() => AmbitionApp.Unsubscribe(GameMessages.FADE_IN_COMPLETE, Activate);
    }
}
