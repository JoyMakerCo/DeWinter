using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class RemarkViewInput : MonoBehaviour
	{
		public int Index;

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
			_remark = (hand.Count > Index) ? hand[Index] : null;
		}

		public void OnClick()
		{
			AmbitionApp.SendMessage<RemarkVO>(_remark);
		}
	}
}
