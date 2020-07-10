using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class MapScene : MonoBehaviour
    {
        public AvatarCollection Avatars;

        public MapView Map { get; private set; }

        public Sprite GetPortrait(string avatarID) => Avatars?.GetAvatar(avatarID).Portrait;

        private void OnEnable()
        {
            if (Map == null)
            {
                PartyModel model = AmbitionApp.GetModel<PartyModel>();
                Map = model.LoadMap(this.transform);
            }
        }
    }
}
