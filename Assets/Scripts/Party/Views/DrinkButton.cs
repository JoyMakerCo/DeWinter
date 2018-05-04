using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class DrinkButton : MonoBehaviour
    {
        private Button _btn;
        void OnEnable()
        {
            _btn = GetComponent<Button>();
            AmbitionApp.Subscribe<int>(GameConsts.DRINK, HandleDrink);
        }
        void OnDisable()
        {
            AmbitionApp.Unsubscribe<int>(GameConsts.DRINK, HandleDrink);
        }
        private void HandleDrink(int amount)
        {
            _btn.enabled = amount > 0;
        }

        public void Drink()
        {
            AmbitionApp.SendMessage(PartyMessages.DRINK);
        }
    }
}
