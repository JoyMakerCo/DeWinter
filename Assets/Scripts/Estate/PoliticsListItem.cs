using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    public class PoliticsListItem : MonoBehaviour
    {
        private const string RELATIONSHIP = "Relationship";
        private const string RELATIONSHIP_EMPTY = "Relationship_Empty";

        private const string POWER = "Power";
        private const string POWER_EMPTY = "Power_Empty";

        private const string CROWN = "Alliegiance_Crown";
        private const string CROWN_EMPTY = "Alliegiance_Crown_Empty";

        private const string REVOLUTION = "Alliegiance_Revolution";
        private const string REVOLUTION_EMPTY = "Alliegiance_Revolution_Empty";

        public SpriteConfig PoliticsSpriteMap;

        public FactionType Faction;
        public Image[] Power;
        public Image[] Allegiance;

        private FactionStandingsVO _faction;
        public FactionStandingsVO Data
        {
            get => _faction;
            set
            {
                FactionModel model = AmbitionApp.GetModel<FactionModel>();
                _faction = value;
                SetArray(_faction.Power, 100, Power, POWER, POWER_EMPTY);
                if (_faction.Allegiance >= 0)
                {
                    SetArray(_faction.Allegiance, 100, Allegiance, CROWN, CROWN_EMPTY);
                }
                else
                {
                    SetArray(-_faction.Allegiance, 100, Allegiance, REVOLUTION, REVOLUTION_EMPTY);
                }
            }
        }

        private void SetArray(int value, int max, Image[] array, string full, string empty)
        {
            Sprite Full = PoliticsSpriteMap.GetSprite(full);
            Sprite Empty = PoliticsSpriteMap.GetSprite(empty);
            bool isFull;
            Color color = Color.white;
            value = Mathf.CeilToInt((float)value * (float)(array.Length) / (float)max);
            for (int i=array.Length; i>0; --i)
            {
                isFull = (i <= value);
                array[i-1].sprite = isFull ? Full : Empty;
                color.a = isFull ? 1 : .5f;
                array[i - 1].color = color;
            }
        }
    }
}