using System;
using System.Linq;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public sealed class UMachineState : UState<UMachine>
	{
		internal UFlowSvc _uflow;	// Active UFlow Service
		internal Dictionary<UState, ULink[]> _states = new Dictionary<UState, ULink[]>();

		// Current active states
		public string[] ActiveStates
		{
			get { return _states.Keys.Select(s=>s.Name).ToArray(); }
		}

		public void Input(string input)
		{
			List<UState> states = new List<UState>();
			List<UState> arriving = new List<UState>();
			List<int> targets = new List<int>();
			UState exit = null;
			foreach(KeyValuePair <UState,ULink[]> state in _states)
			{
				if (!(state.Key is UMachineState))
				{
					List<ULink> links = Array.FindAll(state.Value, l=>l.Input == input && !targets.Contains(l._targetState)).ToList();
					if (links.Count > 0)
					{
						UStateMap[] maps;
						states.Add(state.Key);
						state.Key.OnExitState();
						state.Key.Dispose();
						
						targets.AddRange(links.Select(l=>l._targetState));
						maps = links.Select(l=>Data.Nodes[l._targetState]).ToArray();
						arriving.AddRange(maps.Select(m=>BuildState(m)));
						Array.ForEach(state.Value, l=>{ l.Dispose(); });
					}
				}
			}
			states.ForEach(s=>{ _states.Remove(s); });
			arriving.ForEach(s=>s.OnEnterState());
// LEFT OFF:
// • Resolve immediacy, priority, and default Links
// • Exit a machine and resolve its parent machine
// QUESTIONS:
// - How best to abstact out Link resolution to be used in both normal input and machine exiting
// - Build an intuitive API that uses arbitrary callbacks to trigger inputs and doesn't require explicit flags for immediacy
// - How to get input to resolve first before OnEnterState kicks off the next input
// ANSWER: Create a method queue in UFlow Service
			// Found an Exit state!
			if (exit != null)
			{
				
			}
		}

		override public void OnEnterState()
		{
			UState s = BuildState(Data.Nodes[Data.Initial]);
			s.OnEnterState();
		}

		override public void Dispose()
		{
			_uflow._machines.Remove(this);
			foreach(KeyValuePair<UState, ULink[]> state in _states)
			{
				Array.ForEach(state.Value, l => { l.Dispose(); });
				state.Key.Dispose();
			}
			_states.Clear(); _states = null;
		}

		private UState BuildState(UStateMap map)
		{
			ULinkMap[] maps = Data.GetLinks(map);
			List<ULink> links = new List<ULink>();
			UState state = map._instantiate();
			state._machine = this;
			Array.ForEach(maps, m=>{links.Add(m._instantiate(this, state));});
			_states.Add(state, links.ToArray());
			return state;
		}
	}
}
