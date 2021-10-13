using System;
namespace Ambition
{
    public class CalculateRendezvousResponseCmd : Core.ICommand<RendezVO>
    {
        public void Execute(RendezVO rendez)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            model.Characters.TryGetValue(rendez.Character, out CharacterVO character);
            if (character != null)
            {
                int chance = 2*character.Favor;
                if (Array.IndexOf(character.FavoredLocations, rendez.Location) >= 0)
                {
                    chance += (int)(chance * model.FavoredLocationBonus);
                }
                else if (Array.IndexOf(character.OpposedLocations, rendez.Location) < 0)
                {
                    chance += (int)(chance * model.LocationBonus);
                }
                rendez.RSVP = Util.RNG.Generate(100) < chance ? RSVP.Accepted : RSVP.Declined;
                if (rendez.RSVP == RSVP.Declined) character.LiaisonDay = -1;
                AmbitionApp.SendMessage(rendez);
                AmbitionApp.Calendar.Broadcast();
            }
        }
    }
}
