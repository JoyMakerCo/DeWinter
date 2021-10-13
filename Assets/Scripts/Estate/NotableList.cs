using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class NotableList : SortableList<CharacterVO>
    {
        private CharacterModel _model;

        protected override Comparison<CharacterVO> GetComparer(int sortIndex)
        {
            switch (sortIndex)
            {
                case 1: return SortFaction;
                case 2: return SortFavor;
            }
            return SortAlphabetical;
        }

        private int SortAlphabetical(CharacterVO x, CharacterVO y) => string.Compare(x.ID, y.ID);
        private int SortFaction(CharacterVO x, CharacterVO y) => string.Compare(x.Faction.ToString(), y.Faction.ToString());
        private int SortFavor(CharacterVO x, CharacterVO y) => (x.Favor > y.Favor) ? -1 : (x.Favor == y.Favor) ? 0 : 1;

        protected override CharacterVO[] FetchListData()
        {
            IEnumerable<CharacterVO> characters = AmbitionApp.GetModel<CharacterModel>().Characters.Values;
            List<CharacterVO> list = new List<CharacterVO>(characters).FindAll(c=>c.Acquainted);
            return list.ToArray();
        }
    }
}
