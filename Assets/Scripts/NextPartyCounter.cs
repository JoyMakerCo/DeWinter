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
        public SpriteConfig FactionSymbols;
        public Image FactionSymbol;
        public Text TooltipText;

        private List<PartyVO> _parties;
        private int _index;

        private void Start()
        {
            CalendarModel calendar = AmbitionApp.GetModel<CalendarModel>();
            DateTime endDate = new DateTime(calendar.Today.Year, 7, 14);
            IEnumerable<PartyVO> parties;
            _parties = new List<PartyVO>();
            _index = -1;
            for (DateTime date = calendar.Today; date < endDate; date = date.AddDays(1))
            {
                parties = calendar.GetEvents<PartyVO>(date);
                foreach(PartyVO party in parties)
                {
                    if (party.Attending && FactionSymbols.GetSprite(party.Faction.ToString()) != null)
                    {
                        _parties.Add(party);
                    }
                }
            }
            bool show = _parties.Count > 0;
            gameObject.SetActive(show);
            if (show) ShowNextParty();
        }

        public void ShowNextParty()
        {
            _index = (_index + 1) % _parties.Count;

            var party = _parties[_index];
            //Debug.LogFormat("Party index {0} -> {1}", _index, party);
            FactionType faction = party.Faction;
            if (TooltipText == null)
            {
                Debug.LogWarning("Missing TooltipText in NextPartyCounter");
            }
            else
            {
                TooltipText.text = AmbitionApp.Localize("party_" + faction.ToString().ToLower() + "_likes_and_dislikes");
            }
            FactionSymbol.sprite = FactionSymbols.GetSprite(faction.ToString());
        }
    }
}
