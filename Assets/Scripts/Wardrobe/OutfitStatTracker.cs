using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class OutfitStatTracker : MonoBehaviour
	{
		public string Stat;

		private Slider _meter;

		void Awake()
		{
			_meter = GetComponent<Slider>();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<OutfitVO>(HandleOutfit);
		}

		void OnDisable()
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

