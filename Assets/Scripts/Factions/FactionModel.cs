using System;
using Core;

namespace DeWinter
{
	public class FactionModel : DocumentModel
	{
		public FactionModel () : base("FactionData") {}

		[JsonProperty("factionList")]
		public FactionVO[] Factions;
	}
}