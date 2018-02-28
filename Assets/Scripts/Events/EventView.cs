using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Dialog;

namespace Ambition
{
	public class EventView : DialogView
	{
		public const string DIALOG_ID = "EVENT";
	    public Text titleText;
	    public Text descriptionText;

	    public GameObject LeftAvatar;
		public GameObject CenterAvatar;
		public GameObject RightAvatar;

		public Sprite Background;

	    public override void OnOpen ()
		{
			EventModel model = AmbitionApp.GetModel<EventModel>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
			model.Moment = model.Config.Moments[0];
			titleText.text = model.Config.Name;
		}

		public override void OnClose ()
		{
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
	    }

 		private void HandleMoment(MomentVO moment)
		{
			if (moment != null) descriptionText.text = moment.Text;
		}

	    public void EventOptionSelect(int option)
	    {
			AmbitionApp.SendMessage<int>(EventMessages.EVENT_OPTION, option);
		}
	}
}
