using System;
using System.Collections.Generic;
using UGraph;
using Util;
using UnityEngine;

namespace UFlow
{
    // TODO: Make this a UFlow-exclusive class for setting up flow bindings
    public abstract class UFlowConfig
    {
        private UFlowSvc _uFlow;
        private string _flowID;
        private List<UStateNode> _nodes = new List<UStateNode>();
        private List<Vector2Int> _links = new List<Vector2Int>();

        public abstract void Configure();

        internal DirectedGraph<UStateNode> OnRegister(UFlowSvc svc, string flowID)
        {
            _uFlow = svc;
            _flowID = flowID;
            Configure();
            return new DirectedGraph<UStateNode>()
            {
                Nodes = _nodes.ToArray(),
                Links = _links.ToArray()
            };
        }

        protected void Bind<S>(string stateID) where S : UState, new()
        {
            int index = _nodes.FindIndex(n => n.ID == stateID);
            if (index >= 0) _nodes[index] = new UStateNode<S>() { ID = stateID };
        }

        protected void Bind<S, D>(string stateID, D data) where S : UState, IInitializable<D>, new()
        {
            int index = _nodes.FindIndex(n => n.ID == stateID);
            if (index >= 0) _nodes[index] = new UStateNode<S, D>() { ID = stateID, Data = data };
        }

        protected void Bind<UMachine>(string stateID, string flowID)
        {
            int index = _nodes.FindIndex(n => n.ID == stateID);
            if (index >= 0) _nodes[index] = new UFlowNode() { ID = stateID, Data = flowID };
        }

        // TODO: Move graph definition into editor
        protected void State(string stateID, bool linkPrevState = true)
        {
            if (linkPrevState)
            {
                _links.Add(new Vector2Int(_nodes.Count - 1, _nodes.Count));
            }
            _nodes.Add(new UStateNode() { ID = stateID });
        }

        protected void Link(string state0, string state1)
        {
            Vector2Int link = new Vector2Int(
                _nodes.FindIndex(n => n.ID == state0),
                _nodes.FindIndex(n => n.ID == state1));
            if (!_links.Exists(l => l.Equals(link)))
                _links.Add(link);
        }

        protected void Decision(string stateID, Func<bool> delgate) => Bind<UDecision, Func<bool>>(stateID, delgate);
    }
}
