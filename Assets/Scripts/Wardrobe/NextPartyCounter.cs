using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class NextPartyCounter : MonoBehaviour
    {
        public GameObject NextPartyObject;
        public SpriteConfig FactionSymbols;
        public Text NameText;
        public Text DateText;
        public Image FactionSymbol;
        public GameObject TooltipObject;
        public Text TooltipText;

        public void ShowTooltip(bool show)
        {
            TooltipObject.SetActive(show && !string.IsNullOrEmpty(TooltipText.text));
        }

        private void Start()
        {
            CalendarEvent e = AmbitionApp.GetNextEvent(14);
            NextPartyObject.SetActive(e != null);
            if (e != null)
            {
                bool isParty = e is PartyVO;
                CharacterVO character = isParty ? null : AmbitionApp.GetModel<CharacterModel>().GetCharacter(e.ID);
                string str = (isParty ? ((PartyVO)e).Faction : character?.Faction ?? FactionType.None).ToString();
                FactionSymbol.sprite = FactionSymbols.GetSprite(str);
                TooltipText.text = isParty
                    ? AmbitionApp.Localize("party_" + str.ToLower() + "_likes_and_dislikes")
                    : AmbitionApp.Localize("rendezvous_" + str.ToLower() + "_likes_and_dislikes");
                if (isParty) NameText.text = AmbitionApp.GetModel<LocalizationModel>().GetPartyName(e as PartyVO);
                else
                {
                    str = AmbitionApp.Localize(CharacterConsts.LOC_NAME + (e as RendezVO)?.Character);
                    NameText.text = string.IsNullOrEmpty(str) ? (e as RendezVO)?.Character : str;
                }
                if (DateText != null)
                {
                    DateTime date = AmbitionApp.Calendar.StartDate.AddDays(e.Day);
                    DateText.text = date.Day.ToString() + "/" + date.Month.ToString();
                }
            }
        }
    }
}
