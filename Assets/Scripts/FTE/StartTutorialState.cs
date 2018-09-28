using System;
using UFlow;

namespace Ambition
{
    public class StartTutorialState : TutorialState
    {
        public override void OnEnterState()
        {
			base.OnEnterState();
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			model.Confidence = model.StartConfidence = model.MaxConfidence = 120;

			AmbitionApp.UnregisterCommand<StartTutorialCmd>(GameMessages.START_TUTORIAL);
			AmbitionApp.UnregisterCommand<SkipTutorialCmd>(GameMessages.SKIP_TUTORIAL);
			AmbitionApp.RegisterCommand<TutorialConfidenceCheckCmd>(PartyMessages.SHOW_MAP);
        }
    }
}
