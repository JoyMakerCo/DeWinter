using System;
using Core;

namespace Ambition
{
    public class RegisterIncidentControllerCmd : ICommand
    {
        public void Execute()
        {
            // INCIDENT MACHINE
            AmbitionApp.RegisterState<StartIncidentState>("IncidentController", "StartIncident");
            AmbitionApp.RegisterState<MomentState>("IncidentController", "Moment");
            AmbitionApp.RegisterState<FadeOutState>("IncidentController", "EndIncidentTransition");
            AmbitionApp.RegisterState<EndIncidentState>("IncidentController", "EndIncident");
            AmbitionApp.RegisterState<FadeInState>("IncidentController", "ReturnFromIncident");

            AmbitionApp.RegisterLink("IncidentController", "StartIncident", "Moment");
            AmbitionApp.RegisterLink<CheckEndIncidentLink>("IncidentController", "Moment", "EndIncidentTransition");
            AmbitionApp.RegisterLink<WaitForOptionLink>("IncidentController", "Moment", "Moment");
            AmbitionApp.RegisterLink<AmbitionDelegateLink, string>("IncidentController", "EndIncidentTransition", "EndIncident", GameMessages.FADE_OUT_COMPLETE);
            AmbitionApp.RegisterLink("IncidentController", "EndIncident", "ReturnFromIncident");
        }
    }
}
