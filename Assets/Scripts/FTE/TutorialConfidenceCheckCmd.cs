using System;
using Core;

namespace Ambition
{
	public class TutorialConfidenceCheckCmd : ICommand
	{
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (model.Confidence <= 0)
            {
				AmbitionApp.GetModel<PartyModel>().Confidence = model.StartConfidence;

	            //Explanation Screen Pop Up goes here
				AmbitionApp.OpenMessageDialog("out_of_confidence_tutorial_dialog");
			}
		}
	}
}
