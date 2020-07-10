using System;
using Core;

namespace Ambition
{
    public class LoadGettingCaughtIncidentCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.GetModel<IncidentModel>().Schedule("Getting Caught");
        }
    }
}
