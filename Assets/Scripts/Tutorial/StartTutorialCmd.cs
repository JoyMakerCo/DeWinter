using System;
using UFlow;
namespace Ambition
{
    public class StartTutorialCmd : Core.ICommand<string>
    {
        public void Execute(string tutorialID)
        {
            GameModel game = AmbitionApp.Game;
            if (game.Tutorials.Contains(tutorialID))
            {
                UFlowSvc uflow = AmbitionApp.GetService<UFlowSvc>();
                UMachine machine = uflow.IsActiveFlow(tutorialID)
                    ? null
                    : uflow.Invoke(tutorialID);
            }
        }
    }
}
