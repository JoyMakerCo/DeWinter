using System;
using System.Collections.Generic;
using Util;

namespace Core {
	public interface IAppService : IDisposable { }

    public static class App // This *SHOULD* be replaced by dependency injection. At some point.
    {
        private static Dictionary<Type, IAppService> _services = new Dictionary<Type, IAppService>();

        public static T Register<T>() where T : IAppService, new()
        {
            T svc = Service<T>();
            if (svc == null) _services.Add(typeof(T), svc = new T());
            return svc;
        }

        public static S Register<S>(S svc) where S : IAppService
        {
            _services[typeof(S)] = svc;
            return svc;
        }

        public static bool Unregister<T>() where T:IAppService
        {
            Type t = typeof(T);
            if (!_services.TryGetValue(t, out IAppService svc)) return false;
            svc.Dispose();
            return true;
        }

        public static T Service<T>() where T : IAppService => _services.TryGetValue(typeof(T), out IAppService svc) ? (T)svc : default;
	}
}