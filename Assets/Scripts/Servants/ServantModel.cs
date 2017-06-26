using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace Ambition
{
// TODO: Servants are essentially Inventory Items. Incorporate into Inventory?
	public class ServantModel : DocumentModel
	{
		private Dictionary<string, ServantVO[]> _servants;

		public Dictionary<string, ServantVO> Hired = new Dictionary<string, ServantVO>();
		public Dictionary<string, List<ServantVO>> Introduced = new Dictionary<string, List<ServantVO>>();

		public ServantModel () : base ("ServantData") {}

		[JsonProperty("seamstressDiscount")]
		public float SeamstressDiscount;

		[JsonProperty("servants")]
		public Dictionary<string, ServantVO[]> Servants // Dictionary of Servants by Slot
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

		public ServantVO[] GetServants(string slot)
		{
			ServantVO[] list;
			return _servants.TryGetValue(slot, out list)
				? list
				: new ServantVO[0];
		}
	}
}