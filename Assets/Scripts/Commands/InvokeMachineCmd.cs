using System;
using Core;

namespace Ambition
{
    public class InvokeMachineCmd : ICommand<string>
    {
        public void Execute(string machineID) => AmbitionApp.InvokeMachine(machineID);
    }
}
