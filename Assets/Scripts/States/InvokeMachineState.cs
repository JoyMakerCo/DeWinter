using UFlow;

// TODO: Kill this in favor of submachines
namespace Ambition
{
    public class InvokeMachineState : UState<string>
    {
        override public void OnEnterState()
        {
            AmbitionApp.InvokeMachine(Data);
        }
    }
}
