using System;
using Core;

namespace Ambition
{
	public class DegradeOutfitCmd : ICommand<OutfitVO>
	{
		public void Execute(OutfitVO o)
		{
			InventoryModel model = AmbitionApp.GetModel<InventoryModel>();
			GameModel gm = AmbitionApp.GetModel<GameModel>();
			o.Novelty -= model.NoveltyDamage;
			if (o == gm.LastOutfit)
				o.Novelty -= model.NoveltyDamage;

			gm.LastOutfit = o;
        }
	}
}
