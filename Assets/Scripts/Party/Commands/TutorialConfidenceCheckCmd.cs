using System;
using Core;

namespace Ambition
{
	public class TutorialConfidenceCheckCmd : ICommand<int>
	{
		public void Execute (int confidence)
		{
			if (confidence <= 0)
			{
				PartyModel pmod = AmbitionApp.GetModel<PartyModel>();
				AmbitionApp.AdjustValue(GameConsts.CONFIDENCE, pmod.StartConfidence-confidence);

	            //The Player is relocated to the Entrance
				MapModel mmod = AmbitionApp.GetModel<MapModel>();
	            mmod.Room = mmod.Map.Entrance;

	            //Explanation Screen Pop Up goes here
				AmbitionApp.OpenMessageDialog("out_of_confidence_tutorial_dialog");

	            //The Player is pulled from the Work the Room session
				AmbitionApp.CloseDialog(DialogConsts.ROOM);
				AmbitionApp.CloseDialog(DialogConsts.HOST_ENCOUNTER);
			}
		}
	}
}