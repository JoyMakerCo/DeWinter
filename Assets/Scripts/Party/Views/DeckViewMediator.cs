using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    public class DeckViewMediator : MonoBehaviour
    {
        public Text RemarksCurrentTf;
        public Text RemarksMaxDeckSizeTf;

        private void Awake()
        {
            AmbitionApp.Subscribe<int>(PartyMessages.DECK_SIZE, HandleDeck);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<int>(PartyMessages.DECK_SIZE, HandleDeck);
        }

        // Use this for initialization
        void Start()
        {
            ConversationModel model = AmbitionApp.GetModel<ConversationModel>();
            HandleDeck(model.Deck.Count);
            RemarksMaxDeckSizeTf.text = (model.Deck.Count + model.Discard.Count + Array.FindAll(model.Remarks, r=>r!=null).Length).ToString();
        }

        private void HandleDeck(int deckSize)
        {
            RemarksCurrentTf.text = deckSize.ToString();
        }
    }
}
