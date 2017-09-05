using System;
using System.Collections.Generic;

namespace UFlow
{
	public sealed class UDecision : UState
	{
		public Func<bool>[] conditionals;

		public override void OnEnterState ()
		{
			bool choice;
			UState state=null;
//			int len = Decisions.Length;
//			for (int i=0; state == null && i<len; i++)
//			{
//				if (Machine.Decisions.TryGetValue(Decisions[i][0], out choice) && choice)
//				{
//					state = _UFlow.BuildState(Decisions[i][1]);
//				}
//			}
//			if (state == null)
//				state = _UFlow.BuildState(Decisions[0][1]);
			Machine.SetState("");
		}
	}
}
