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
	public class IncidentScene : MonoBehaviour
	{
		public const string DIALOG_ID = "INCIDENT";
	    public Text titleText;
	    public Text descriptionText;
		public Text SpeakerName;

	    public AvatarView Character1;
		public AvatarView Character2;
		private Image _background;
		private AudioSource[] _audio;

	    void Awake ()
		{
			_background = gameObject.GetComponent<Image>();
			AmbitionApp.Subscribe<MomentVO>(HandleMoment);
		}

		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
	    }

		void OnEnable()
		{
			IncidentModel model = AmbitionApp.GetModel<IncidentModel>();
			titleText.text = model.Incident.Name;
		}

 		private void HandleMoment(MomentVO moment)
		{
			if (moment != null)
			{
				descriptionText.text = moment.Text;
				if (moment.Background != null) _background.sprite=moment.Background;
				Character1.ID = moment.Character1.AvatarID;
				Character1.Pose = moment.Character1.Pose;
				Character2.ID = moment.Character2.AvatarID;
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
