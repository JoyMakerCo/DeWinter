using System;
using Core;

namespace Ambition
{
    public class DrawCmd : ICommand<int>
    {
        public void Execute(int numcards)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            ConversationModel conversation = AmbitionApp.GetModel<ConversationModel>();
            RemarkVO[] hand = conversation.Remarks;
            int num = Array.FindAll(hand, r => r == null).Length;
            if (num < numcards) numcards = num;
            if (model.Deck.Count < numcards) numcards = model.Deck.Count;
            for (int i = 0; i < numcards; i++)
            {
                num = Array.IndexOf(hand, null);
                hand[num] = model.Deck.Dequeue();
                AmbitionApp.SendMessage(PartyMessages.DRAW_REMARK, hand[num]);
            }
            AmbitionApp.SendMessage(PartyMessages.DECK_SIZE, model.Deck.Count);
            conversation.Remarks = hand;
            conversation.Remark = null;
        }
    }
}
