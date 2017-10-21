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
			AmbitionApp.Subscribe<OutfitVO>(HandleOutfit);
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<OutfitVO>(HandleOutfit);
		}

		private void HandleOutfit(OutfitVO outfit)
		{
			_animator.SetFloat("OutfitNovelty", (float)outfit.Novelty*.01f);
		}
	}
}
