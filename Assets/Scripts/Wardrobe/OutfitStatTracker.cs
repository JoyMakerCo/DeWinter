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
			AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		void OnEnable()
		{
			PartyVO party = AmbitionApp.GetModel<PartyModel>().Party;
			if (party != null)
			{
				FactionVO faction = AmbitionApp.GetModel<FactionModel>()[party.Faction];
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
			AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		private void HandleOutfit(ItemVO outfit)
		{
            _meter.value = outfit?.Type == ItemType.Outfit
                ? OutfitWrapperVO.GetIntStat(outfit, Stat)
                : 0;
		}
	}
}
