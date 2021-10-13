using System;
namespace Ambition
{
    public class EndTutorialCmd : Core.ICommand<string>
    {
        public void Execute(string tutorialID)
        {
            AmbitionApp.Game.Tutorials.RemoveAll(t => t == tutorialID);
            AmbitionApp.GetService<UFlow.UFlowSvc>().Unregister(tutorialID);
        }
    }
}
