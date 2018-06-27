using System;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEditor;


namespace UFlow
{
    public class UController : MonoBehaviour
    {
        public string MachineID; // TODO: Make state configurations ScriptableObjects so that Machines can just be dropped in
        public UStateDelegateMap [] Delegates;
        public string[] Tags;
        internal UMachine _machine;

        [Serializable]
        public class UStateDelegateMap
        {
            public string StateID;
            public UControllerDelegate Delegate = new UControllerDelegate();
        }

        void Start()
        {
            if (_machine == null)
            {
                App.Service<UFlowSvc>().BuildController(this);
    			if (_machine != null) _machine.OnEnterState();
            }
        }

        internal void Invoke(string stateID)
        {
            UStateDelegateMap map = Array.Find(Delegates, m=>m.StateID == stateID);
            if (map != null) map.Delegate.Invoke();
        }

        internal void Invoke<T>(string stateID)
        {
            UStateDelegateMap map = Array.Find(Delegates, m=>m.StateID == stateID);
            if (map != null) map.Delegate.Invoke();
        }

        void OnDestroy()
        {
            if (_machine != null) _machine.Dispose();
            _machine = null;
        }
    }
}
