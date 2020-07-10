using System;
using Core;
using UFlow;

namespace Ambition
{
    public class EndGameState : UState
    {
        private const string END_INCIDENT = "Thanks For Playing";
        public override void OnEnterState()
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule(END_INCIDENT);
        }
    }
}
