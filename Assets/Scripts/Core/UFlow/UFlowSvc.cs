using System;
using System.Collections.Generic;
using Core;
using Util;

namespace uflow
{
	public interface IUFlowBindings
	{
//		Dictionary<string, delegate> SetBindings(string json);
	}

	public class UFlowSvc : IAppService, IInitializable, IDisposable
	{
		public UFlowSvc () {}

		public uState State;
		public uMachine Machine
		{
			get { return State.Machine;} 
		}

		public void Initialize()
		{
			App.Service<MessageSvc>().Subscribe(UFlowMessages.NEXT_STATE, HandleNextState);
			App.Service<MessageSvc>().Subscribe<string>(UFlowMessages.NEXT_STATE, HandleDecisionState);
		}

		public void Dispose()
		{
			App.Service<MessageSvc>().Unsubscribe(UFlowMessages.NEXT_STATE, HandleNextState);
			App.Service<MessageSvc>().Unsubscribe<string>(UFlowMessages.NEXT_STATE, HandleDecisionState);
		}

		public uMachine Build(string file, IUFlowBindings bindings)
		{
			uMachine machine = new uMachine();
			return machine;
		}

		public void HandleNextState()
		{
			State.OnExitState();
			State.NextState.OnEnterState(State);
			State = State.NextState;
		}

		public void HandleDecisionState(string Decision)
		{
			HandleNextState();
			if (State is UDecisionState)
			{
				(State as UDecisionState).Decision = Decision;
				HandleNextState();
			}
		}
	}
}
