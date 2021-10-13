using System;
namespace Ambition
{
    public class CharacterReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO commodity)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            CharacterVO character = model.GetCharacter(commodity.ID);
            if (character != null)
            {
                character.Acquainted = commodity.Value > 0;
                model.Broadcast();

                if (!character.IsDateable && character.IsRendezvousScheduled)
                {
                    RendezVO[] rendezs = AmbitionApp.Calendar.GetOccasions<RendezVO>(character.LiaisonDay);
                    RendezVO rendez = Array.Find(rendezs, r => r.Character == commodity.ID);
                    AmbitionApp.Calendar.Remove(rendez);
                }
            }
        }
    }
}
