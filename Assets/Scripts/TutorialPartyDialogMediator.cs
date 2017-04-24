using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialog;

namespace DeWinter
{
	public class TutorialPartyDialogMediator : MonoBehaviour
	{
		public const string DIALOG_ID = "WorkTheRoomTutorialPopUpModal";

		void OnDestroy()
		{
			PartyModel model = DeWinterApp.GetModel<PartyModel>();
			DeWinterApp.SendMessage<float>(PartyMessages.START_TIMERS, model.TurnTimer);
		}
	}
}