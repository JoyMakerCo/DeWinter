using System;
using Core;
using UnityEngine;

namespace Ambition
{
    public class FavorReward : ICommand<CommodityVO>
    {
        public void Execute(CommodityVO favor)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            CharacterVO character = model.GetCharacter(favor.ID);
            if (character != null)
            {
                character.Favor += favor.Value;
                if (character.Favor < 0) character.Favor = 0;
                else if (character.Favor > 100) character.Favor = 100;
            }
#if DEBUG
            else Debug.LogWarningFormat("FavorReward.Execute: unrecognized character ID '{0}", favor.ID);
#endif
        }
    }
}
