using System;
using UnityEngine.UI;

namespace Ambition
{
    public class LocationDialogMediator : Dialog.DialogView<LocationConfig>, ISubmitHandler, IDisposable
    { 
        public const string DIALOG_ID = "PARIS_LOCATION";

        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Text GoButtonText;
        public Image LocationImage;
        public RewardRangeList List;

        public FMODEvent ClickSound;
        public FMODEvent CancelSound;

        private string _location;
        private IncidentVO _incident;

        public override void OnOpen(LocationConfig config)
        {
            bool rendezvousMode = AmbitionApp.GetModel<CharacterModel>().CreateRendezvousMode;
            ParisModel paris = AmbitionApp.Paris;
            IncidentModel story = AmbitionApp.Story;
            LocationVO location = paris.SaveLocation(config);
            _incident = story.GetIncident(config.IntroIncidentConfig?.name);

            _location = config.name;
            LocationNameText.text = AmbitionApp.Localize(ParisConsts.LABEL + _location);
            LocationDescriptionText.text = AmbitionApp.Localize(ParisConsts.DESCRIPTION + _location);
            LocationImage.sprite = config.LocationModalSprite;
            GoButtonText.text = AmbitionApp.Localize(rendezvousMode
                ? ParisConsts.PICK_LOC_BTN_LABEL
                : ParisConsts.GO_LOC_BTN_LABEL);

            if (!AmbitionApp.CheckIncidentEligible(_incident) && config.StoryIncidentConfigs != null)
            {
                foreach (IncidentConfig icon in config.StoryIncidentConfigs)
                {
                    if (AmbitionApp.CheckIncidentEligible(icon?.name))
                    {
                        _incident = story.GetIncident(icon.name);
                    }
                }
            }

            List.SetIncident(_incident);
        }

        public void Cancel()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY, CancelSound);
            Close();
        }

        public void Submit()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY, ClickSound);
            AmbitionApp.SendMessage(ParisMessages.CHOOSE_LOCATION, _location);
            AmbitionApp.SendMessage(ParisMessages.CHOOSE_LOCATION);
            Close();
        }

        public void Dispose() => _location = null;
    }
}
