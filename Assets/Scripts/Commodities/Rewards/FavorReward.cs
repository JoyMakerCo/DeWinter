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
            if (!model.Characters.TryGetValue(favor.ID, out CharacterVO character))
            {
                Debug.LogWarningFormat("FavorReward.Execute: unrecognized character ID '{0}', inventing that character now", favor.ID);

                model.Characters.Add(favor.ID, character = new CharacterVO(favor.ID, new AvatarVO()));
            }
            character.Favor += favor.Value;
            if (character.Favor < 0) character.Favor = 0;
            else if (character.Favor > 100) character.Favor = 100;
        }
    }
}
