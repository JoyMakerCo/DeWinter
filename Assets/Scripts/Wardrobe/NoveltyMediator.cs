using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class NoveltyMediator : MonoBehaviour
	{
		private Animator _animator;

		void Awake ()
		{
			_animator = GetComponent<Animator>();
		}

		void OnEnable()
		{
			AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleOutfit);
		}

		private void HandleOutfit(ItemVO outfit)
		{
            int val = (outfit?.Type == ItemType.Outfit)
                ? OutfitWrapperVO.GetNovelty(outfit)
                : 0;
            _animator.SetFloat("OutfitNovelty", .01f*(float)val);
		}
	}
}
