using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Ambition
{
	[Serializable]
	public struct RemarkViewConfig
	{
		public Image Profile;
		public Image Icon;
	}

	public class RemarkViewMediator : MonoBehaviour
	{
		public RemarkViewConfig[] Remarks;
		private List<RemarkVO> _hand;
		private PartyArtLibrary _library;

		// Use this for initialization
		void Awake ()
		{
			AmbitionApp.Subscribe<List<RemarkVO>>(HandleHand);
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<List<RemarkVO>>(HandleHand);
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
		}

		void Start()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			_library = this.gameObject.GetComponent<PartyArtLibrary>();
			HandleHand(model.Hand);
		}

		private void HandleHand(List<RemarkVO> hand)
		{
			bool isActive;
			RemarkMap profile;
			InterestMap interest;
			_hand = hand;
			for(int i=Remarks.Length-1; i>=0; i--)
			{
				isActive = (i<hand.Count);
				Remarks[i].Icon.enabled = isActive;
				Remarks[i].Profile.enabled = isActive;
				if (isActive)
				{
					profile = Array.Find(_library.RemarkSprites, r=>r.ID == hand[i].Profile);
					Remarks[i].Profile.sprite = (default(RemarkMap).Equals(profile)) ? profile.Sprite : null;
					interest = Array.Find(_library.InterestSprites, n=>n.ID == hand[i].Interest);
					Remarks[i].Icon.sprite = (default(InterestMap).Equals(interest)) ? interest.Sprite : null;
				}
			}
		}

		private void HandleRemark(RemarkVO remark)
		{
			Color c;
			for(int i=Remarks.Length-1; i>=0; i--)
			{
				c = (remark == null || i >= _hand.Count || _hand[i] == remark) ? Color.white : Color.grey;
				Remarks[i].Profile.color = c;
				Remarks[i].Icon.color = c;
			}
		}
	}
}
