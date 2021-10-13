using Core;
namespace Ambition
{
    public class GoToPartyCmd : ICommand<PartyVO>
    {
        public void Execute(PartyVO party)
        {
            if (party != null)
            {
                if (AmbitionApp.Inventory.GetEquippedItem(ItemType.Outfit) != null)
                {
                    AmbitionApp.SendMessage(PartyMessages.START_PARTY);
                }
                else
                {
                    //You ain't got no clothes to attend the party! 
                    AmbitionApp.OpenDialog(DialogConsts.NO_OUTFIT);
                }
            }
        }
    }
}
