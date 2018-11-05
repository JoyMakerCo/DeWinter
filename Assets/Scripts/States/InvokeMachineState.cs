using UFlow;

// TODO: Kill this in favor of submachines
namespace Ambition
{
    public class InvokeMachineState : UState<string>
    {
        private string _machineID;
        override public void SetData(string machineID) => _machineID = machineID;
        override public void OnEnterState() => AmbitionApp.InvokeMachine(_machineID);
    }
}
