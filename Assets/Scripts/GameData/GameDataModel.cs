using System;
using System.Collections.Generic;
using Core;

namespace DeWinter
{
	public class GameDataModel : IModel
	{
		public Dictionary<string, double> Balances;

		public GameDataModel()
		{
			Balances = new Dictionary<string, double>();
		}
	}
}