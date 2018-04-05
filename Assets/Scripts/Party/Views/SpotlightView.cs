using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SpotlightView : MonoBehaviour
    {
        public Util.ColorConfig Colors;
        public GuestView Guest;
        private Image _image;
        private RemarkVO _remark;
        private bool _intoxicated;
        
        void Awake()
        {
            _image = GetComponent<Image>();
            _image.enabled = false;
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleGuests);
            AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleGuests);
            AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
        }

        private void HandleRemark(RemarkVO remark)
        {
            float alpha = _image.color.a;
            Color c;
            _remark = remark;
            _image.enabled = false;
            if (_remark == null) c = Colors.GetColor("Neutral");
            else if (_remark.Interest == Guest.Guest.Like) c = Colors.GetColor("Like");
            else if (_remark.Interest == Guest.Guest.Disike) c = Colors.GetColor("Dislike");
            else c = Colors.GetColor("Neutral");
            c.a = alpha;
            _image.color = c;
        }

        private void HandleGuests(GuestVO [] guests)
        {
			_image.enabled = _remark != null && guests != null && !_intoxicated && Array.IndexOf(guests, Guest.Guest) >= 0;
        }

        private void HandleIntoxication(int tox)
		{
			_intoxicated = (tox >= 50);
		}
    }
}
