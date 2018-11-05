using System;
using Core;
using Util;
namespace Ambition
{
    /// <summary>
    ///  Reshuffle the specified number of cards from the discard pile to the draw deck
    /// </summary>
    public class ReshuffleCmd : ICommand<int>
    {
        public void Execute(int numCards)
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            int index;
            for (int i = (model.Discard.Count < numCards) ? model.Discard.Count : numCards; i > 0; i--)
            {
                index = RNG.Generate(model.Discard.Count);
                model.Deck.Enqueue(model.Discard[index]);
                model.Discard.RemoveAt(index);
            }
        }
    }
}
