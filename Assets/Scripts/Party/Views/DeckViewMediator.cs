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
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            HandleDeck(model.Deck.Count);
            RemarksMaxDeckSizeTf.text = model.MaxDeckSize.ToString();
        }

        private void HandleDeck(int deckSize)
        {
            RemarksCurrentTf.text = deckSize.ToString();
        }
    }
}
