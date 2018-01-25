using System;
using System.Collections.Generic;

namespace Core
{
	public interface IFabricator {}
	public interface IFabricator<F,P> : IFabricator
	{
		P Create(F parms);
	}

	public class FactorySvc : IAppService
	{
		protected Dictionary<string, List<Delegate>> _fabricators = new Dictionary<string, List<Delegate>>();

		public void Register<Param,Product,Fabricator>() where Fabricator:IFabricator<Param,Product>, new()
		{
			Register<Param,Product,Fabricator>("");
		}

		public void Register<Param,Product,Fabricator>(string key) where Fabricator:IFabricator<Param,Product>, new()
		{
			List<Delegate> delegates;
			if (!_fabricators.TryGetValue(key, out delegates))
			{
				_fabricators.Add(key, new List<Delegate>());
			}
			else if (delegates.Find(d=>d is Func<Param,Product>) != null)
			{
				throw new Exception("> Fabricator already exists!");
			}
			Func<Param,Product> fabricator = fb=>new Fabricator().Create(fb);
			_fabricators[key].Add(fabricator);
		}

		public Product Create<Param,Product>(Param fabricationParameter)
		{
			return Create<Param,Product>("", fabricationParameter);
		}

		public Product Create<Param,Product>(string key, Param fabricationParameter)
		{
			List<Delegate> delegates;
			if (!_fabricators.TryGetValue(key, out delegates))
				return default(Product);
			Func<Param,Product> del = delegates.Find(d=>d is Func<Param,Product>) as Func<Param,Product>;
			return (del != null)
				? del(fabricationParameter)
				: default(Product);
		}
	}
}
