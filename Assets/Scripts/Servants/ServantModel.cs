using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
	public class ServantModel : DocumentModel
	{
		public Dictionary<string, ServantVO> Hired = new Dictionary<string, ServantVO>();
		public List<ServantVO> Introduced = new List<ServantVO>();

		public ServantModel () : base ("ServantData") {}

		public ServantVO[] Servants
		{
			private set;
			get;
		}

		[JsonProperty("servants")]
		private ServantVO[] _servants
		{
			set
			{
				Servants = value;
				foreach(ServantVO servant in value)
				{
					if (servant.Introduced || servant.Hired)
					{
						Introduced.Add(servant);
						if (servant.Hired)
						{
							Hired[servant.Slot] = servant;
						}
					}
				}
			}
		}
	}
}
