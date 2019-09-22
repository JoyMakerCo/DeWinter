using System;
using System.Collections.Generic;
using System.Linq;
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
        public AmbitionLocalizedText TooltipText;

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
                parties = parties.Where(p => p.Attending && FactionSymbols.GetSprite(p.Faction.ToString()) != null);
                _parties.AddRange(parties);
            }
            bool show = _parties.Count > 0;
            gameObject.SetActive(show);
            if (show) ShowNextParty();
        }

        public void ShowNextParty()
        {
            _index = (_index + 1) % _parties.Count;
            FactionType faction = _parties[_index].Faction;
            TooltipText.Localize("party_" + faction.ToString().ToLower() + "_likes_and_dislikes");
            FactionSymbol.sprite = FactionSymbols.GetSprite(faction.ToString());
        }
    }
}
