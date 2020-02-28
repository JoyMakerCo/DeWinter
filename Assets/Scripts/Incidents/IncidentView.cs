using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Dialog;

namespace Ambition
{
	public class IncidentView : MonoBehaviour
	{
	    public Text descriptionText;
		public Text SpeakerName;
	    public AvatarView Character1;
		public AvatarView Character2;
		public RawImage Background;

        private string _localizationKey;

	    void Awake ()
		{
            AmbitionApp.Subscribe<MomentVO>(HandleMoment);
            AmbitionApp.Subscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
        }

        void OnDestroy ()
		{
            AmbitionApp.Unsubscribe<MomentVO>(HandleMoment);
            AmbitionApp.Unsubscribe<IncidentVO>(IncidentMessages.START_INCIDENT, HandleIncident);
        }

        private void HandleIncident(IncidentVO incident) => _localizationKey = incident?.LocalizationKey;

        private void HandleMoment(MomentVO moment)
		{
            if (moment != null)
            {
                descriptionText.text = AmbitionApp.Localize(_localizationKey + ".node." + moment.Index.ToString());
                if (moment.Background != null) Background.texture = moment.Background.texture;
                Character1.ID = moment.Character1.AvatarID;
                Character1.Pose = moment.Character1.Pose;
                Character2.ID = moment.Character2.AvatarID;
                Character2.Pose = moment.Character2.Pose;

                if (moment.Music.Name.Length > 0)
                    AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, moment.Music);
                if (moment.AmbientSFX.Name.Length > 0)
                    AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, moment.AmbientSFX);
                if (moment.OneShotSFX.Name.Length > 0)
                    AmbitionApp.SendMessage(AudioMessages.PLAY, moment.OneShotSFX);

                SpeakerName.enabled = (moment.Speaker != SpeakerType.None);
                switch (moment.Speaker)
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
            else
            {
                Debug.Log("How did I get here??");
            }
        }
	}
}
