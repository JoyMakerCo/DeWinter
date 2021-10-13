using Steamworks;
namespace Ambition
{
    public static class SteamAchievementReq
    {
        public static bool Check(RequirementVO req)
        {
            int val = !string.IsNullOrEmpty(req.ID)
                && SteamAPI.IsSteamRunning()
                && Steamworks.SteamUserStats.GetAchievement(req.ID, out bool done)
                && done
                ? 1
                : 0;
            return RequirementsSvc.Check(req, val);
        }
    }
}
