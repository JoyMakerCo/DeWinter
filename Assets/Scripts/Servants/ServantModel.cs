using System;
using System.Collections.Generic;
using Core;
using Newtonsoft.Json;

namespace DeWinter
{
	public class ServantModel : DocumentModel
	{
		public ServantModel () : base ("ServantData") {}

		public Dictionary<string, ServantVO> Servants = new Dictionary<string, ServantVO>();

		[JsonProperty("seamstressDiscount")]
		public float SeamstressDiscount;

		public ServantVO GetServant(string servantName)
		{
			return Array.Find(_servantData, p => p.name == servantName);
		}

		[JsonProperty("servants")]
		private ServantVO[] _servantData;
	}
}