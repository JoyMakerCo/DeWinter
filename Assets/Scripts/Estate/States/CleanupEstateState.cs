using System;
namespace Ambition
{
    public class CleanupEstateState : UFlow.UState
    {
        public override void OnEnter()
        {
            AmbitionApp.GetModel<CharacterModel>().CreateRendezvous = null;
        }
    }
}
