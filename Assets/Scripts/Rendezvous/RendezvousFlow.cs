using System;
using UFlow;
namespace Ambition
{
    public class RendezvousFlow : UFlowConfig
    {
        public override void Configure()
        {
            State("InitRendezvous");
            State("RendezvousIncident");
            State("PostRendezvous");

            Bind<InitRendezvousState>("InitRendezvous");
            Bind<UMachine>("RendezvousIncident", FlowConsts.INCIDENT_CONTROLLER);
            Bind<PostRendezvousState>("PostRendezvous");
        }
    }
}
