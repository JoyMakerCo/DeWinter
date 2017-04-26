using System;
using UnityEngine;
using UnityEngine.UI;

namespace DeWinter
{
	public class RemarkViewMediator : MonoBehaviour
	{
		// TODO: Test to see if carrying around references to prefabs increases memory usage
		private EncounterViewMediator _encounterView;
		private Image _profileImage;
		private Image _dispositionImage;

		void Start()
		{
			DeWinterApp.Subscribe<RemarkVO>(PartyMessages.REMARK_SELECTED, HandleRemarkSelected);
			_encounterView = GetComponentInParent<EncounterViewMediator>();
			_profileImage = GetComponent<Image>();
			_dispositionImage = GetComponentInChildren<Image>();
		}

		void OnDestroy()
		{
			DeWinterApp.Unsubscribe<RemarkVO>(PartyMessages.REMARK_SELECTED, HandleRemarkSelected);
		}

		private void HandleRemarkSelected(RemarkVO remark)
		{
			_profileImage.color = (remark == _remark) ? Color.white : Color.gray;
		}

		void OnMouseDown()
	    {
	        DeWinterApp.SendMessage<RemarkVO>(PartyMessages.REMARK_SELECTED, _remark);
	    }

		private RemarkVO _remark;
		public RemarkVO Remark
		{
			get { return _remark; }
			set {
				_remark = value;
				_profileImage.sprite = Array.Find(_encounterView.RemarkSprites, r => r.Key == _remark.Profile);
				_dispositionImage.sprite = Array.Find(_encounterView.Dispositions, d => d.Key == Remark.Disposition.name);
			}
		}
	}
}