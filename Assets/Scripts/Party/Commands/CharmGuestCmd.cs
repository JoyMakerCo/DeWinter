using System;
using Core;

namespace Ambition
{
    public class CharmGuestCmd : ICommand<CharacterVO>
    {
        public void Execute(CharacterVO guest)
        {
            //guest.Opinion = 100;
            //guest.State = GuestState.Charmed;
            PartyModel partyModel = AmbitionApp.GetModel<PartyModel>();
            for (int bonusRemark = partyModel.CharmedRemarkBonus; bonusRemark > 0; bonusRemark--)
            {
                AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
            }
        }
    }
}
