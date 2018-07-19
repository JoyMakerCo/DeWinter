using System;
using Core;

namespace Ambition
{
    public class RefillDrinkCmd : ICommand
    {
        public void Execute()
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            model.Drink = model.MaxDrinkAmount;
        }
    }
}
