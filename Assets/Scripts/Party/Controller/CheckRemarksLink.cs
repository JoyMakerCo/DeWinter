using System;
using UFlow;
namespace Ambition
{
    public class CheckRemarksLink : ULink
    {
        public override bool Validate()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            return AmbitionApp.GetModel<PartyModel>().Deck.Count > 0
                || (model.Remarks != null
                && Array.Exists(model.Remarks, r => r != null));
        }
    }
}
