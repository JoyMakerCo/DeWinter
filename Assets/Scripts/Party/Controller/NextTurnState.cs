using System;
using UFlow;
namespace Ambition
{
    public class NextTurnState : UState
    {
        public override void OnEnter()
        {
            AmbitionApp.SendMessage(PartyMessages.TURN, ++AmbitionApp.GetModel<PartyModel>().Turn);
        }
    }
}
