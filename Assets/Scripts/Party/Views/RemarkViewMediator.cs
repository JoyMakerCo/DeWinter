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
		private const float SLIDE_TIME = 0.2f;

		public RemarkViewConfig[] Remarks;
		private List<RemarkVO> _hand;
		private PartyArtLibrary _library;
		private Vector3[] _iconPositions = new Vector3[3];
		private Vector3[] _profilePositions = new Vector3[3];

		// Use this for initialization
		void Awake ()
		{
			_library = this.gameObject.GetComponent<PartyArtLibrary>();
			AmbitionApp.Subscribe<List<RemarkVO>>(HandleHand);
			AmbitionApp.Subscribe<RemarkVO>(HandleRemark);

			_iconPositions[0] = Remarks[0].Icon.transform.position;
			_iconPositions[1] = Remarks[Remarks.Length-1].Icon.transform.position;
			_iconPositions[2] = (_iconPositions[1] - _iconPositions[0])*(1f/(float)(Remarks.Length-1));
			_profilePositions[0] = Remarks[0].Profile.transform.position;
			_profilePositions[1] = Remarks[Remarks.Length-1].Profile.transform.position;
			_profilePositions[2] = (_profilePositions[1] - _profilePositions[0])*(1f/(float)(Remarks.Length-1));
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<List<RemarkVO>>(HandleHand);
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
			StopAllCoroutines();
		}

		private void HandleHand(List<RemarkVO> hand)
		{
			bool isActive;
			RemarkMap map;
			_hand = hand;
			StopAllCoroutines();
			if (!gameObject.active) return;
			for(int i=Remarks.Length-1; i>=0; i--)
			{
				isActive = (hand.Count > i);
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

						StartCoroutine(Slide(i, hand.Count));
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
				c = (remark == null || i >= _hand.Count || _hand[i] == remark) ? Color.white : Color.grey;
				Remarks[i].Profile.color = c;
				Remarks[i].Icon.color = c;
			}
		}

		IEnumerator Slide(int index, int numCards)
		{
			Vector3 [] positions = new Vector3[]{
				Remarks[index].Icon.transform.position,
				_iconPositions[0] + _iconPositions[2]*(float)(.5f*(float)(Remarks.Length - numCards) + index),
				Remarks[index].Profile.transform.position,
				_profilePositions[0] + _profilePositions[2]*(float)(.5f*(float)(Remarks.Length - numCards) + index)
			};
			for (float t=0; t<SLIDE_TIME; t+=Time.deltaTime)
			{
				Remarks[index].Icon.transform.position = Vector3.Lerp(positions[0], positions[1], t/SLIDE_TIME);
				Remarks[index].Profile.transform.position = Vector3.Lerp(positions[2], positions[3], t/SLIDE_TIME);
				yield return null;
			}
			Remarks[index].Icon.transform.position = positions[1];
			Remarks[index].Profile.transform.position = positions[3];
		}
	}
}
