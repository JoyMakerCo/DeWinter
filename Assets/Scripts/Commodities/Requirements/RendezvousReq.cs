using System;
namespace Ambition
{
    public static class RendezvousReq
    {
        // If currently on a rendezvous, checks the requirement ID against both the current character and location
        // If not on a rendezvous, checks whether the requirement ID is either a viable rendezvous destination or a dateable character
        // A value of 1 or greater verifies a match;
        // A value of 0 or less verifies that neither match
        public static bool Check(RequirementVO requirement)
        {
            CharacterModel model = AmbitionApp.GetModel<CharacterModel>();
            if (AmbitionApp.GetModel<GameModel>().Activity == ActivityType.Rendezvous && model.Rendezvous != null)
            {
                return (model.Rendezvous.Character == requirement.ID || model.Rendezvous.Location == requirement.ID) == (requirement.Value > 0);
            }
            if (AmbitionApp.Paris.Completed.Contains(requirement.ID)) return false;
            if (AmbitionApp.Paris.Rendezvous.Contains(requirement.ID)) return true;
            if (!model.Characters.TryGetValue(requirement.ID, out CharacterVO character)) return false;
            return character?.IsDateable ?? false;
        }
    }
}
