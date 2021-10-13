namespace Ambition
{
    public class SteamAchievementReward : Core.ICommand<CommodityVO>
    {
        public void Execute(CommodityVO reward)
        {
            if (!string.IsNullOrEmpty(reward.ID)
                && reward.Value > 0
                && Steamworks.SteamAPI.IsSteamRunning()
                && Steamworks.SteamUserStats.GetAchievement(reward.ID, out bool done)
                && !done)
            {
                Steamworks.SteamUserStats.SetAchievement(reward.ID);
                Steamworks.SteamUserStats.StoreStats();
            }
        }
    }
}
