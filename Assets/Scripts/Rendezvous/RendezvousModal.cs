using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RendezvousModal : Dialog.DialogView<DateTime>, ISubmitHandler
    {
        public RendezvousListItem ListItem;
        public SpriteConfig LiaisonConfig;
        public Button ConfirmButton;

        private RendezVO _rendez;

        public void OnSelectLiaison(RendezvousListItem listItem)
        {
            _rendez.ID = listItem?.Character?.ID;
            ConfirmButton.interactable = !string.IsNullOrEmpty(_rendez.Character);
        }

        public override void OnOpen(DateTime date)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            GameObject obj;
            RendezvousListItem item = ListItem;
            AmbitionApp.GetModel<CharacterModel>().CreateRendezvous = null;

            _rendez = new RendezVO()
            {
                Created = AmbitionApp.Calendar.Day,
                RSVP = RSVP.New,
                Day = date.Subtract(AmbitionApp.Calendar.StartDate).Days,
                IsCaller = true
            };

            foreach (CharacterVO character in model.Characters.Values)
            {
                if (character.IsDateable && !character.IsRendezvousScheduled)
                {
                    if (item == null)
                    {
                        obj = Instantiate(ListItem.gameObject, ListItem.transform.parent);
                        item = obj.GetComponent<RendezvousListItem>();
                    }
                    item?.SetCharacter(character, LiaisonConfig.GetSprite(character.Faction.ToString()));
                    item = null;
                }
            }
        }

        public void Cancel() => Close();
        public void Submit()
        {
            AmbitionApp.GetModel<CharacterModel>().CreateRendezvous = _rendez;
            AmbitionApp.SendMessage(RendezvousMessages.CHOOSE_RENDEZVOUS);
            Close();
        }
    }
}
