using System;
using System.Collections.Generic;
using Core; 

namespace Ambition
{
	public class SetFashionCmd : ICommand<PartyVO>
	{
		public void Execute (PartyVO party)
		{
            if (AmbitionApp.GetModel<GameModel>().Level > 8)
            {
                InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
                ItemVO item;
                if (inventory.Equipped.TryGetValue(ItemConsts.OUTFIT, out item) && item != null)
                {
                    string style = (item as OutfitVO).Style;
                    if (inventory.Equipped.TryGetValue(ItemConsts.ACCESSORY, out item)
                        && item != null && (string)item.State[ItemConsts.STYLE] == style)
                    {
                        if (Util.RNG.Generate(0, 4) == 0)
                        {
                            //Send Out a Relevant Pop-Up
                            Dictionary<string, string> substitutions = new Dictionary<string, string>(){
                                {"$OLDSTYLE",inventory.CurrentStyle},
                                {"$NEWSTYLE",style}};
                            AmbitionApp.OpenMessageDialog("set_trend_dialog", substitutions);
                        }
                    }
                }
            }
		}
	}
}