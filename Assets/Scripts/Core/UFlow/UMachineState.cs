using System;
namespace UFlow
{
    public sealed class UMachineState : UNode
    {
        public string MachineID { get; internal set; }
        public UMachineState(string stateID, string machineID)
        {
            ID = stateID;
            MachineID = machineID;
        }

        private UMachine _subMachine;
        public override void Cleanup()
        {
            _subMachine?.Cleanup();
            _subMachine = null;
        }

        public override void OnEnterState(string[] args)
        {
            if (_subMachine == null)
            {
                _subMachine = _UFlow.BuildMachine(MachineID);
                _subMachine._State = this;
            }
            if (_subMachine != null) _subMachine?.Start();
            else _Machine?.Activate(this);
        }
    }
}
