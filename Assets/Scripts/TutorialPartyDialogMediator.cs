using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

namespace Ambition
{
	public class TutorialPartyDialogMediator : MonoBehaviour
	{
		public const string DIALOG_ID = "WorkTheRoomTutorialPopUpModal";

		void OnDestroy()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			AmbitionApp.SendMessage(PartyMessages.START_TIMERS);
		}
	}
}