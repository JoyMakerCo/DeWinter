using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
    [Serializable]
    public class UMachineGraph : DirectedGraph<UStateNode, UGraphLink> {}

	public class UMachine : UState<string>
	{
        
		public string MachineID { get; private set; }
		internal UFlowSvc _uflow;	// Active UFlow Service
		internal UMachineGraph _graph;
		internal UState[] _states;
		private ULink[][] _links;
		private Queue<int> _queue = new Queue<int>();

		override public void SetData(string data)
		{
			MachineID = data;
		}

		// Current active states
		public string[] GetActiveStates()
		{
			return (from s in _states
				where s != null
				select s.ID).ToArray();
		}

		internal void Activate(ULink link)
		{
			// TODO: Aggregate links
			_queue.Enqueue(link._target);
			ExitState(link._origin);
			if (_queue.Count == 1) Activate();
		}

		private void Activate()
		{
			UState state;
			ULink[] links;
			UStateNode node;
			int index;
			while (_queue.Count > 0)
			{
				index = _queue.Peek();
				state = _states[index];
				node = _graph.Nodes[_queue.Peek()];
				if (state == null)
				{
					state = _uflow.Build<UStateNode, UState>(node);
					state._machine = this;
					state._node = node;
					_states[index] = state;
				}
				links = _links[index];
				if (links == null)
				{
					links = node is UStateNode<UMachine, string> ? new ULink[0] : BuildLinks(index);
					_links[index] = links;
				}
				state.OnEnterState();

// UnityEngine.Debug.Log(_uflow.ToString());

				if (links.Length > 0)
				{
					links = Array.FindAll(links, l=>l.Validate());
					Array.ForEach(links, Activate);
					if (links.Length > 0) ExitState(index);
					_queue.Dequeue();
				}
				// Found an Exit State!
				// TODO: Implement Exit/persistent states
				// else if (persistent) _queue.Dequeue();
				else
				{
					ExitState(index);
					ExitMachine();
				}
			}

	// TODO:
	// - In the UI, outgoing links are shown in a reorderable list where list order = priority
	// - Default links are always resolved last (greyed out if possible)
		}

		override public void OnEnterState()
		{
			int length = _graph.Nodes.Length;
			if (length > 0)
			{
				if (_states == null) _states = new UState[length];
				if (_links == null) _links = new ULink[length][];
				_queue.Enqueue(0);
				Activate();
			}
			else ExitMachine();
		}

		private void ExitState(int index)
		{
			if (_links[index] != null)
			{
				Array.ForEach(_links[index], l=>l.Dispose());
				_links[index] = null;
			}
			if (_states[index] != null)
			{
				_states[index].OnExitState();
				_states[index].Dispose();
				_states[index] = null;
			}
		}

		private void Clear()
		{
			if (_links != null)
				foreach (ULink[] links in _links)
					if (links != null)
						Array.ForEach(links, l=>l.Dispose());
			if (_states != null)
				Array.ForEach(_states, s=>{ if (s!=null) s.Dispose(); });
			if (_queue != null)
				_queue.Clear();
		}

		private void ExitMachine()
		{
			Clear();
			_uflow.RemoveMachine(this);
			if (_machine != null) 
			{
				int index = Array.FindIndex(_machine._graph.Nodes, n=>n.ID == this.ID && n is UStateNode<UMachine, string>);
				ULink[] links = _machine.BuildLinks(index);
				_machine._links[index] = links;
				links = Array.FindAll(_machine._links[index], l=>l.Validate());
				Array.ForEach(links, _machine.Activate);
			}
		}

		override public void Dispose()
		{
			Clear();
			_links = null;
			_states = null;
			_queue = null;
			_machine = null;				
			_uflow.RemoveMachine(this);
		}

		private ULink[] BuildLinks(int index)
		{
			UGraphLink[] links = _graph.GetLinkData(index);
            List<ULink> uLinks = new List<ULink>();
            ULink link;
            foreach(UGraphLink l in links)
            {
                link = BuildLink(l);
                uLinks.Add(link);
            }
            return uLinks.ToArray();// .Select(link=>BuildLink(link)).ToArray();
		}

		private ULink BuildLink(UGraphLink link)
		{
			ULink ln = _uflow.Build<UGraphLink, ULink>(link);
			int linkIndex = Array.IndexOf(_graph.LinkData, link);
			ln._origin = _graph.Links[linkIndex].x;
			ln._target = _graph.Links[linkIndex].y;
			ln._machine = this;
			ln.Initialize();
			return ln;
		}

		public override string ToString()
		{
			return "UFlow::UMachine " + MachineID + ": " + string.Join(", ", GetActiveStates());
		}
	}
}
