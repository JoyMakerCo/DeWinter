using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RoomDialog : Dialog.DialogView<IncidentVO>, ISubmitHandler
    {
        public const string DIALOG_ID = "ROOM_DIALOG";
        public Text LocationNameText;
        public Text LocationDescriptionText;
        public Image LocationImage;
        public GameObject RewardsContainer;
        public RewardRangeList RewardsList;
        public BackgroundConfig BGConfig;

        private Dictionary<string, RewardItem> _rewardItems;
        private Dictionary<string, Vector2Int> _rewardRanges;
        private IncidentVO _incident;

        public override void OnOpen(IncidentVO incident)
        {
            _incident = incident;
            _rewardItems = new Dictionary<string, RewardItem>();
            _rewardRanges = new Dictionary<string, Vector2Int>();
            LocationNameText.text = AmbitionApp.Localize(incident.ID + ".name");
            LocationDescriptionText.text = AmbitionApp.Localize(incident.ID + ".description");
            if (incident?.Nodes != null && incident.Nodes.Length > 0)
            {
                BackgroundConfig.BackgroundMap bg;
                foreach (MomentVO moment in incident.Nodes)
                {
                    if (moment.Background != null)
                    {
                        bg = Array.Find(BGConfig.Backgrounds, b => b.IncidentBackground == moment.Background);
                        if (bg.ModalBackground != null)
                        {
                            LocationImage.sprite = bg.ModalBackground;
                            break;
                        }
                    }
                }
                RewardsList.SetIncident(incident);
            }
        }

        public void Cancel() => Close();
        public void Submit()
        {
            AmbitionApp.SendMessage(PartyMessages.SHOW_ROOM, _incident?.ID);
            Close();
        }
    }
}
