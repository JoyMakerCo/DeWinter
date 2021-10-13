using System;
namespace Ambition
{
    public class DeclineRendezvousCmd : Core.ICommand<RendezVO>
    {
        public void Execute(RendezVO rendez)
        {
            if (rendez.RSVP != RSVP.Required)
            {
                AmbitionApp.GetModel<CharacterModel>().Characters.TryGetValue(rendez.Character, out CharacterVO character);
                rendez.RSVP = RSVP.Declined;
                if (character != null)
                {
                    if (character.LiaisonDay == rendez.Day) character.LiaisonDay = -1;
                    if (character.Rejections < 4) ++character.Rejections;
                    CommodityVO penalty = new CommodityVO()
                    {
                        Type = CommodityType.Favor,
                        ID = rendez.Character,
                        Value = -character.Rejections
                    };
                    AmbitionApp.SendMessage(penalty);
                    AmbitionApp.GetModel<CharacterModel>().Broadcast();
                }
                AmbitionApp.SendMessage(rendez);
            }
        }
    }
}
