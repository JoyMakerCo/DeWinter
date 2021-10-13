using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RoomView : MonoBehaviour
    {
        private int MAX_PORTRAITS = 4;

        public PortraitView Portrait;
        public Text ButtonText;

        private IncidentVO _incident = null;

        [SerializeField]
        private AvatarCollection avatarCollection;

        public void SetIncident(IncidentVO incident)
        {
            _incident = incident;
            gameObject.SetActive(incident?.Nodes != null);
            if (incident?.Nodes == null) return;

            int nodeCount = incident.Nodes.Length;
            List<string> characters = new List<string>();
            MomentVO moment;

            for (int i=0; i<nodeCount && characters.Count < MAX_PORTRAITS; ++i)
            {
                moment = incident.Nodes[i];
                SetAvatar(moment.Character1, characters);
                SetAvatar(moment.Character2, characters);
            }
        }

        public void ShowRoom() => AmbitionApp.OpenDialog(RoomDialog.DIALOG_ID, _incident);

        private void SetAvatar(IncidentCharacterConfig character, List<string> characters)
        {
            string characterName = character.Name;
            if (characters.Count < MAX_PORTRAITS
                && !string.IsNullOrWhiteSpace(characterName)
                && !string.IsNullOrWhiteSpace(character.AvatarID)
                && !characters.Contains(characterName))
            {
                AvatarConfig avatar = avatarCollection.GetAvatar(character.AvatarID);
                if (avatar?.Portrait != null)
                {
                    PortraitView view;
                    characters.Add(characterName);
                    if (characters.Count > 1)
                    {
                        GameObject obj = GameObject.Instantiate<GameObject>(Portrait.gameObject, Portrait.transform.parent);
                        view = obj.GetComponent<PortraitView>();
                    }
                    else
                    {
                        view = Portrait;
                    }
                    view.gameObject.SetActive(true);
                    view.Tooltip = characterName;
                    view.Portrait.sprite = avatar.Portrait;
                }
            }
        }
    }
}
