using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class RemarkViewMediator : MonoBehaviour
	{
		public int index;

		// Art library prefab
		// TODO: Test to see if carrying around references to prefabs increases memory usage
		// TODO: Non-gameobject config files?

		private PartyArtLibrary _library;
		private Image _profileImage;
		private Image _dispositionImage;
		private RemarkVO _remark;

		void Start()
		{
			AmbitionApp.Subscribe<RemarkVO>(HandleRemarkSelected);
			AmbitionApp.Subscribe<List<RemarkVO>>(HandleHandUpdated);
//			PartyArtLibrary _library = ArtLibrary.GetComponent<PartyArtLibrary>();
			_profileImage = GetComponent<Image>();
			_dispositionImage = GetComponentInChildren<Image>();
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<RemarkVO>(HandleRemarkSelected);
			AmbitionApp.Unsubscribe<List<RemarkVO>>(HandleHandUpdated);
		}

		private void HandleRemarkSelected(RemarkVO remark)
		{
			_profileImage.color = (remark == _remark || remark == null) ? Color.white : Color.gray;
		}

		void OnMouseDown()
	    {
	        AmbitionApp.SendMessage<RemarkVO>(_remark);
	    }

		private void HandleHandUpdated(List<RemarkVO> hand)
		{
			_remark = (hand.Count > index) ? hand[index] : null;
			this.gameObject.SetActive(_remark != null);
			if (_remark != null)
			{
				_remark = hand[index];
//				_profileImage.sprite = Array.Find(_library.RemarkSprites, r => r.X == _remark.Profile).Y;
//				_dispositionImage.sprite = Array.Find(_library.InterestSprites, d => d.X == _remark.Interest).Y;
			}
		}
	}
}