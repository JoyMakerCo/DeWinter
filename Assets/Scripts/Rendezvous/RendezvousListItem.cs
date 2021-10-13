using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class RendezvousListItem : MonoBehaviour
    {
        public Image Portrait;
        public Text NameText;

        public CharacterVO Character { get; private set; }
        public void SetCharacter(CharacterVO character, Sprite icon = null)
        {
            Character = character;
            NameText.text = AmbitionApp.GetModel<LocalizationModel>().GetFullName(character) ?? character.ID;
            if (icon != null) Portrait.sprite = icon;
        }
    }
}
