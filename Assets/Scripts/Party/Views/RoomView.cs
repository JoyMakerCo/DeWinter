using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RoomView : MonoBehaviour
    {
        public PortraitView[] PortraitsEven;
        public PortraitView[] PortraitsOdd;
        public Button RoomButton;

        private PartyModel _model;
        private IncidentVO _incident;

        [SerializeField]
        private AvatarCollection avatarCollection;

        void Awake()
        {
            int[] counts = new int[2];
            _model = AmbitionApp.GetModel<PartyModel>();
        }

        private void OnEnable() => RoomButton?.onClick.AddListener(OnClick);
        private void OnDisable() => RoomButton?.onClick.RemoveListener(OnClick);

        public void SetIncident(IncidentVO incident)
        {
            _incident = incident;
            gameObject.SetActive(_incident != null);
            if (_incident == null) return;

            List<CharacterVO> characters = new List<CharacterVO>();
            string name;
            AvatarVO avatar;
            PortraitView[] portraits;
            int max = PortraitsEven.Length > PortraitsOdd.Length ? PortraitsEven.Length : PortraitsOdd.Length;
            int i = 0;

            foreach (MomentVO moment in _incident.Nodes)
            {
                name = moment.Character1.Name;
                if (!string.IsNullOrWhiteSpace(name) && !characters.Exists(c => c.Name == name))
                {
                    avatar = avatarCollection.GetAvatar(moment.Character1.AvatarID);
                    if (avatar.Portrait != null)
                        characters.Add(new CharacterVO(name, avatar));
                }
                name = moment.Character2.Name;
                if (!string.IsNullOrWhiteSpace(name) && !characters.Exists(c=>c.Name == name) && characters.Count<max)
                {
                    avatar = avatarCollection.GetAvatar(moment.Character2.AvatarID);
                    if (avatar.Portrait != null)
                        characters.Add(new CharacterVO(name, avatar));
                }

                if (characters.Count == max) break;
            }
            portraits = characters.Count % 2 == 0 ? PortraitsEven : PortraitsOdd;
            characters.ForEach(c => portraits[i++].Character = c);
            while (i<portraits.Length)
            {
                portraits[i++].gameObject.SetActive(false);
            }
            portraits = (portraits == PortraitsEven) ? PortraitsOdd : PortraitsEven;
            Array.ForEach(portraits, p => p.gameObject.SetActive(false));
        }

        private void OnClick() => AmbitionApp.SendMessage(PartyMessages.SHOW_ROOM, _incident);
    }
}
