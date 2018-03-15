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

                Dictionary<string, string> subs = new Dictionary<string, string>() {
                {"$FACTION", AmbitionApp.GetString(model.Party.Faction)},
                {"$REPUTATION", "25"},
                {"$FACTIONREPUTATION", "50"}};
                AmbitionApp.OpenMessageDialog("out_of_confidence_dialog", subs);
            }
        }
    }
}
