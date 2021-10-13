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
			AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleOutfit);
		}

		private void HandleOutfit(ItemVO item)
		{
            OutfitVO outfit = item as OutfitVO;
            int val = outfit?.Novelty ?? 0;
            _animator.SetFloat("OutfitNovelty", .01f*(float)val);
		}
	}
}
