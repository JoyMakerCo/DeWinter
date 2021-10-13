using System;
namespace Ambition
{
    public class GetInventoryCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.SendMessage(AmbitionApp.Inventory.Inventory);
        }
    }
}
