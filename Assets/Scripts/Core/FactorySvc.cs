using System;
using System.Collections.Generic;

namespace Core
{
	public interface IFactory {}
	public interface IFactory<Key, Product> : IFactory
	{
		Product Create(Key key);
	}

	public class FactorySvc : IAppService
	{
		// USAGE:
		// App.Service<FactorySvc>().Register<Param, Result>(param, func);
		// App.Service<FactorySvc>().Create<Param, Result>(param);

		private Dictionary<Type, IFactory> _factories = new Dictionary<Type, IFactory>();

		public IFactory<Key, Product> GetFactory<Key, Product>()
		{
			IFactory factory;
			_factories.TryGetValue(typeof(IFactory<Key, Product>), out factory);
			return factory as IFactory<Key, Product>;
		}

		public T Create<T>() where T:new() { return new T(); }

		public Product Create<Key, Product>(Key key)
		{
			Type type = typeof(IFactory<Key, Product>);
			IFactory factory;
			return _factories.TryGetValue(type, out factory)
				? ((IFactory<Key, Product>)factory).Create(key)
				: default(Product);
		}

		public void Register<Key, Product>(IFactory<Key, Product> factory)
		{
			_factories[typeof(IFactory<Key, Product>)] = factory;
		}
	}
}
