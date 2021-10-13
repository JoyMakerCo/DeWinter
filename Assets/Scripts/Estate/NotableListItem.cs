using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class NotableListItem : SortableItem<CharacterVO>
    {
        public AvatarCollection AvatarConfig;
        public SpriteConfig FactionSprites;
        public Image PortraitImage;
        public Image FactionImage;
        public Image ParamourIcon;
        public Text HeaderText;
        public Text DescriptionText;

        private CharacterVO _character;
        public override CharacterVO Data
        {
            get => _character;
            set
            {
                _character = value; // Will throw an exception if null
                PortraitImage.sprite = AvatarConfig.GetPortrait(_character.Avatar);
                HeaderText.text = AmbitionApp.Localize(CharacterConsts.LOC_FULL_NAME + _character.ID);
                FactionImage.sprite = FactionSprites.GetSprite(_character.Faction.ToString().ToLower());
                FactionImage.enabled = FactionImage.sprite != null;
                ParamourIcon.enabled = _character.IsDateable;
                DescriptionText.text = AmbitionApp.Localize(CharacterConsts.LOC_DESCRIPTION + _character.ID);
            }
        }
    }
}
