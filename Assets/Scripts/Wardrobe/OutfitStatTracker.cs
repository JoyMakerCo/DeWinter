using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class OutfitStatTracker : MonoBehaviour
	{
		public string Stat;
		public Image Minimum;
		public Image Maximum;

		private Slider _meter;

		void Awake()
		{
			_meter = GetComponent<Slider>();
			AmbitionApp.Subscribe<OutfitVO>(HandleOutfit);
		}

		void OnEnable()
		{
			string factionID = AmbitionApp.GetModel<PartyModel>().Party.Faction;
			FactionVO faction = AmbitionApp.GetModel<FactionModel>()[factionID];
			int stat = (Stat.ToLower() == ItemConsts.LUXURY ? faction.Luxury : faction.Modesty);
			Minimum.enabled = (stat < 0);
			Maximum.enabled = (stat > 0);

		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<OutfitVO>(HandleOutfit);
		}

		private void HandleOutfit(OutfitVO outfit)
		{
			if (outfit != null)
			{
				ItemVO item = (ItemVO)outfit;
				object value;
				_meter.value = item.State.TryGetValue(Stat, out value)
					? Convert.ToInt32(value)
					: 0;
			}
			else _meter.value = 0;
		}
	}
}
