using System;
namespace Ambition
{
    public class ReturnToEstateState : UFlow.UState
    {
        public override void OnEnter() => AmbitionApp.Paris.Location = null;
    }
}
