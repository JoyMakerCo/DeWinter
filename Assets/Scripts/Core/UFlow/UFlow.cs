using System;
using System.Collections.Generic;
using Core;
using Util;

namespace UFlow
{
	public interface IUFlowBindings
	{
	}

	public class UFlowSvc : IAppService, IInitializable
	{
		private Dictionary<string,Dictionary<string, Func<UState>>> _bindings = new Dictionary<string, Dictionary<string, Func<UState>>>();

		public void Initialize()
		{
			// TODO: Read the machine from config
			string file;
			IUFlowBindings bindings;
		}

		public void RegisterState<S>(string stateID, string machineID) where S : UState, new()
		{
			Type s = typeof(S);
			Dictionary<string, Func<UState>> bindings;
			if (!_bindings.TryGetValue(machineID, out bindings))
			{
				bindings = new Dictionary<string, Func<UState>>();
				_bindings.Add(machineID, bindings);
			}
			bindings[stateID] = () => {
				S state = new S();
				state._uflow = this;
				return new S();
			};
		}

		internal UState BuildState(string stateID, string machineID)
		{
			try
			{
				return _bindings[machineID][stateID].Invoke();
			}
			catch(Exception e)
			{
				throw new Exception("No binding exists for state \'" + stateID + "\' in machine \'" + machineID + "\'", e);
			}
		}
	}
}
