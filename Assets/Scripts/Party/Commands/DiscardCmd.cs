using System;
using Core;
namespace Ambition
{
    public class DiscardCmd : ICommand<RemarkVO>
    {
        public void Execute(RemarkVO remark)
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            if (model.Remarks != null && remark != null)
            {
                int index = Array.IndexOf(model.Remarks, remark);
                if (index >= 0)
                {
                    model.Remarks[index] = null;
                    if (!remark.Free) model.Discard.Add(remark);
                }
            }
            AmbitionApp.SendMessage(PartyMessages.DECK_SIZE, model.Deck.Count);
        }
    }
}
