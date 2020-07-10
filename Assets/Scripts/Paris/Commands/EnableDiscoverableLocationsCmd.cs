using System;
namespace Ambition
{
    public class EnableDiscoverableLocationsCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterCommand<ChooseExploreLocationsCmd, string[]>(ParisMessages.SELECT_DAILIES);
        }
    }

    public class DisableDiscoverableLocationsCmd : Core.ICommand
    {
        public void Execute()
        {
            AmbitionApp.UnregisterCommand<ChooseExploreLocationsCmd, string[]>(ParisMessages.SELECT_DAILIES);
        }
    }
}
