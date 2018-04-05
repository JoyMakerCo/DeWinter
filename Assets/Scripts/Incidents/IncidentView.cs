using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Dialog;

namespace Ambition
{
	public class IncidentView : DialogView
	{
		public const string DIALOG_ID = "INCIDENT";
	    public Text titleText;
	    public Text descriptionText;
		public Text SpeakerName;

	    public AvatarView Character1;
		public AvatarView Character2;
		private Image _background;
		private AudioSource[] _audio;

	    public override void OnOpen ()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			_background = gameObject.GetComponent<Image>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
			model.Moment = model.Incident.Moments[0];
			titleText.text = model.Incident.Name;
		}

		public override void OnClose ()
		{
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
			AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
	    }

 		private void HandleMoment(MomentVO moment)
		{
			if (moment != null)
			{
				descriptionText.text = moment.Text;
				if (moment.Background != null) _background.sprite=moment.Background;
				Character1.AvatarID = moment.Character1.AvatarID;
				Character1.Pose = moment.Character1.Pose;
				Character2.AvatarID = moment.Character2.AvatarID;
				Character2.Pose = moment.Character2.Pose;

				AmbitionApp.SendMessage<AmbientClip>(AudioMessages.PLAY_MUSIC, moment.Music);
				SpeakerName.enabled = (moment.Speaker != SpeakerType.None);
				switch(moment.Speaker)
				{
					case SpeakerType.Player:
						SpeakerName.text = AmbitionApp.GetModel<GameModel>().PlayerName;
						break;
					case SpeakerType.Character1:
						SpeakerName.text = moment.Character1.Name;
						break;
					case SpeakerType.Character2:
						SpeakerName.text = moment.Character2.Name;
						break;
				}
			}
		}
	}
}
