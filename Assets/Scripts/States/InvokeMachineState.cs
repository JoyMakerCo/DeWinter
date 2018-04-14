using UFlow;

// TODO: Kill this in favor of submachines
namespace Ambition
{
    public class InvokeMachineState : UState, Util.IInitializable<string>
    {
        private string _mac;
        public void Initialize(string machine)
        {
            _mac = machine;
        }

        override public void OnEnterState()
        {
            AmbitionApp.InvokeMachine(_mac);
        }
    }
}
