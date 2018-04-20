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
            _remark = remark;
            _image.enabled = false;
            if (Guest != null && Guest.Guest != null && _remark != null)
            {
                Color c;
                float alpha = _image.color.a;
                if (_remark.Interest == Guest.Guest.Like) c = Colors.GetColor("Like");
                else if (_remark.Interest == Guest.Guest.Dislike) c = Colors.GetColor("Dislike");
                else c = Colors.GetColor("Neutral");
                c.a = alpha;
                _image.color = c;
            }
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
