using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;

namespace Ambition
{
	public class DevotionModel : DocumentModel, Util.IInitializable, IDisposable
	{
		[JsonProperty("SeductionMod")]
		public int SeductionModifier;

		[JsonProperty("SeductionAltMod")]
		public int SeductionAltModifier;

		[JsonProperty("SeductionMarriageMod")]
		public int SeductionMarriedModifier;

		[JsonProperty("SeductionTimeMod")]
		public int SeductionTimeModifier;

		[JsonProperty("SeductionDevotion")]
		public int SeductionDevotion;

		[JsonProperty("Notables")]
		public Dictionary <string, NotableVO> Notables;

		public DevotionModel() : base("Devotion") {}

		public void Initialize()
		{
			AmbitionApp.Subscribe<RequestAdjustValueVO<int>>(HandleDevotion);
		}

		public void Dispose()
		{
			AmbitionApp.Unsubscribe<RequestAdjustValueVO<int>>(HandleDevotion);
		}	

		private void HandleDevotion(RequestAdjustValueVO<int> vo)
		{
			NotableVO notable;
			if (Notables.TryGetValue(vo.Type, out notable))
			{
				notable.Devotion += (int)vo.Value;
				Notables[vo.Type] = notable;
				AmbitionApp.SendMessage<NotableVO>(notable);
			}

		}
	}
}