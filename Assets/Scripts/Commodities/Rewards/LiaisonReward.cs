using System;
using System.Collections.Generic;
using Util;
namespace Ambition
{
    public class LiaisonReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            model.Characters.TryGetValue(reward.ID, out CharacterVO character);
            if (character != null)
            {
                character.IsDateable = (reward.Value > 0);
                if (reward.Value > 0) character.Acquainted = true;
            }
        }
    }
}
