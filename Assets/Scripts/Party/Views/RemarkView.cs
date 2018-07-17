using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ambition
{
	public class RemarkView : MonoBehaviour, IPointerClickHandler
	{
		public int Index;
        public Animator RemarkAnimator;

		private RemarkVO _remark;

		void Awake ()
		{
			AmbitionApp.Subscribe<List<RemarkVO>>(HandleHand);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<List<RemarkVO>>(HandleHand);
		}

		private void HandleHand(List<RemarkVO> hand)
		{
			_remark = (Index < hand.Count) ? hand[Index] : null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			AmbitionApp.GetModel<ConversationModel>().Remark = _remark;
		}
	}
}
