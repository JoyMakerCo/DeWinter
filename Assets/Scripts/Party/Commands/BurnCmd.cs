using System;
using Core;

namespace Ambition
{
    public class BurnCmd : ICommand<int>
    {
        public void Execute(int numCards)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            for (int i = numCards > model.Deck.Count ? model.Deck.Count : numCards; i > 0; i--)
            {
                model.Discard.Add(model.Deck.Dequeue());
            }
            AmbitionApp.SendMessage(PartyMessages.DECK_SIZE, model.Deck.Count);
        }
    }
}
