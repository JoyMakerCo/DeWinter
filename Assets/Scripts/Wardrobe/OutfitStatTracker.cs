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
			PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
			if (party != null)
			{
				string factionID = party == null ? null : party.Faction;
				FactionVO faction = AmbitionApp.GetModel<FactionModel>()[factionID];
				int stat = (Stat.ToLower() == ItemConsts.LUXURY ? faction.Luxury : faction.Modesty);
				Minimum.enabled = (stat < 0);
				Maximum.enabled = (stat > 0);
			}
			else
			{
				Minimum.enabled = false;
				Maximum.enabled = false;
			}
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
