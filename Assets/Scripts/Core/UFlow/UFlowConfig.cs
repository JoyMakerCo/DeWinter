using System;
using System.Collections.Generic;
using Util;

namespace UFlow
{
    public abstract class UFlowConfig
    // TODO: Make this a UFlow-exclusive class for setting up flow bindings
     : Core.ICommand<string>
    {
        internal UFlowSvc _uFlow;
        internal string _flowID;

        private string _prevState = null;

        public abstract void Initialize();

        // TODO: Remove when this is no longer a command
        public void Execute(string flowID)
        {
            _flowID = flowID;
            _uFlow = Core.App.Service<UFlowSvc>();
            Initialize();
        }

        // TODO: Move Bindings into UFlow
        protected void Bind<S>(string stateID) where S : UState, new()
        {
            _uFlow.BindState<S>(_flowID, stateID);
        }

        protected void Bind<S, D>(string stateID, D data) where S : UState, IInitializable<D>, new()
        {
            _uFlow.BindState<S, D>(_flowID, stateID, data);
        }

        // TODO: ULinks no longer bindable, just states
        // For now, Bindlink also creates links if they don't already exist
        protected void BindLink<L>(string fromState, string toState) where L : ULink, new()
        {
            _uFlow.BindLink<L>(_flowID, fromState, toState);
        }

        protected void BindLink<L, D>(string fromState, string toState, D data) where L : ULink, IInitializable<D>, new()
        {
            _uFlow.BindLink<L, D>(_flowID, fromState, toState, data);
        }

        // TODO: Move graph definition into editor
        private string _prev; 
        protected void State(string stateID, bool linkPrevState = true)
        {
            _uFlow.RegisterState(_flowID, stateID);
            if (linkPrevState && _prev != null)
                _uFlow.RegisterLink(_flowID, _prev, stateID);
            _prev = stateID;
        }

        protected void Link(string state0, string state1)
        {
            _uFlow.RegisterLink(_flowID, state0, state1);
            _prev = state1;
        }
    }
}
