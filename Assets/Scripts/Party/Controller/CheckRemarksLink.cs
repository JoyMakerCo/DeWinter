using System;
using UFlow;
namespace Ambition
{
    public class CheckRemarksLink : ULink
    {
        public override bool Validate()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            return model.Deck.Count == 0 && (model.Remarks == null || Array.TrueForAll(model.Remarks, r => r == null));
        }
    }
}
