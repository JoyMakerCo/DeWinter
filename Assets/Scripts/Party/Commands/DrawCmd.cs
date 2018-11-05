using System;
using Core;

namespace Ambition
{
    public class DrawCmd : ICommand<int>
    {
        public void Execute(int numcards)
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            RemarkVO[] hand = model.Remarks;
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
            model.Remarks = hand;
            model.Remark = null;
        }
    }
}
