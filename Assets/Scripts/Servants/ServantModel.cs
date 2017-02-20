using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class ServantModel : DocumentModel
	{
		private Dictionary<string, ServantVO[]> _servants;

		public ServantModel () : base ("ServantData") {}

		[JsonProperty("seamstressDiscount")]
		public float SeamstressDiscount;

		[JsonProperty("servants")]
		public Dictionary<string, ServantVO[]> Servants
		{
			get { return _servants; }
			set {
				_servants = value;
				foreach(KeyValuePair<string, ServantVO[]> kvp in _servants)
				{
					for (int i=kvp.Value.Length-1; i>=0; i--)
					{
						_servants[kvp.Key][i].slot = kvp.Key;
					}
				}
			}
		}

		public ServantVO GetHired(string slot)
		{
			ServantVO[] list;
			if (_servants.TryGetValue(slot, out list))
			{
				return Array.Find(list, s => s.Hired);
			}
			return null;
		}

		public ServantVO[] GetIntroduced(string slot)
		{
			ServantVO[] list;
			if (_servants.TryGetValue(slot, out list))
			{
				return Array.FindAll(list, s => s.Introduced && !s.Hired);
			}
			return new ServantVO[0];
		}

		public ServantVO[] GetServants(string slot)
		{
			ServantVO[] list;
			return _servants.TryGetValue(slot, out list)
				? list
				: new ServantVO[0];
		}
	}
}