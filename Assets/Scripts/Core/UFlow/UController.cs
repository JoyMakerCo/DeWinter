using System;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEditor;


// Deprecated, unless needed
namespace UFlow
{
    public class UController : MonoBehaviour
    {
        public string MachineID; // TODO: Make state configurations ScriptableObjects so that Machines can just be dropped in
        public UStateDelegateMap [] Delegates;
        public string[] Tags;
        internal UMachine _Machine;
        internal UFlowSvc _UFlow;

        [Serializable]
        public class UStateDelegateMap
        {
            public string StateID;
            public UControllerDelegate Delegate = new UControllerDelegate();
        }

        void Start()
        {
            _UFlow = App.Service<UFlowSvc>();
            //_UFlow.Activate(this);
            //if (_Machine != null) _Machine.Start();
            //else _Machine = _UFlow.InvokeMachine(MachineID);
        }

        internal void Invoke(string stateID)
        {
            UStateDelegateMap map = Array.Find(Delegates, m=>m.StateID == stateID);
            map?.Delegate?.Invoke();
        }

        internal void Invoke<T>(string stateID)
        {
            UStateDelegateMap map = Array.Find(Delegates, m=>m.StateID == stateID);
            map?.Delegate?.Invoke();
        }

//        void OnDestroy() => _UFlow?.Remove(this);
    }
}
