using Core;
using System.Collections.Generic;

namespace Ambition
{
    public class OutOfConfidenceDialogCmd : ICommand
    {
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            if (model.Confidence <= 0)
            {
                AmbitionApp.GetModel<PartyModel>().Confidence = model.StartConfidence >> 1;

                CommodityVO[] penalties = new CommodityVO[]
                {
                    new CommodityVO(CommodityType.Reputation, -25),
                    new CommodityVO(CommodityType.Faction, model.Party.Faction, -50)
                };
                AmbitionApp.OpenDialog<CommodityVO[]>("DEFEAT", penalties);
            }
        }
    }
}
