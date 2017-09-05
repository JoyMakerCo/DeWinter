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
		private RemarkVO[] _hand;
		private PartyArtLibrary _library;

		// Use this for initialization
		void Awake ()
		{
			_library = this.gameObject.GetComponent<PartyArtLibrary>();
			AmbitionApp.Subscribe<RemarkVO []>(HandleHand);
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<RemarkVO []>(HandleHand);
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
		}

		private void HandleHand(RemarkVO[] hand)
		{
			bool isActive;
			RemarkMap map;
			_hand = hand;
			for(int i=Remarks.Length-1; i>=0; i--)
			{
				isActive = (hand[i] != null);
				if (isActive)
				{
					map = Array.Find(_library.RemarkSprites, n=>n.Interest == hand[i].Interest);
					if (default(RemarkMap).Equals(map))
					{
						isActive = false;
					}
					else
					{
						Remarks[i].Icon.sprite = map.InterestSprite;
						Remarks[i].Profile.sprite = map.TargetSprites[hand[i].NumTargets-1];
					}
				}
				Remarks[i].Icon.enabled = isActive;
				Remarks[i].Profile.enabled = isActive;
			}
		}

		private void HandleRemark(RemarkVO remark)
		{
			Color c;
			for(int i=Remarks.Length-1; i>=0; i--)
			{
				c = (remark == null || _hand[i] == remark) ? Color.white : Color.grey;
				Remarks[i].Profile.color = c;
				Remarks[i].Icon.color = c;
			}
		}
	}
}
