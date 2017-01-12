using System;
using System.Collections.Generic;
using Util;

namespace Core {
	public interface IAppService {}

	public static class App
	{
		private static Dictionary<Type, IAppService> _services = new Dictionary<Type, IAppService>();

		public static T Service<T>() where T:IAppService, new()
		{
			Type t = typeof(T);
			IAppService svc;
			if (!_services.TryGetValue(t, out svc))
			{
				svc = new T();
				_services[t] = svc;
				if (svc is IInitializable)
					(svc as IInitializable).Initialize();
			}
			return (T)svc;
		}
	}
}