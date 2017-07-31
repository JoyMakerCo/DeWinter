using System;
using System.Collections.Generic;

namespace Core
{
	public class ValueSvc : IAppService
	{
		private Dictionary<string, ITrackableValue> _values;
		public void Register<T>(string valueID)
		{
			
		}

		public void RegisterClass<T, U>() where U : TrackableValue<T>
		{
			
		}
	}
}

