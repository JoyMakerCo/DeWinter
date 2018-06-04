using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SpotlightView : MonoBehaviour
    {
        public Util.ColorConfig Colors;
        public Image[] SpotImages;
        private bool _intoxicated;
        private GuestVO _guest;
        private int _index;
        private bool _on;
        void Awake()
        {
            _index = transform.GetSiblingIndex();
            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Subscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleGuestsTargeted);
            AmbitionApp.Subscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
            On = false;
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Unsubscribe<GuestVO[]>(HandleGuests);
            AmbitionApp.Unsubscribe<GuestVO[]>(PartyMessages.GUESTS_TARGETED, HandleGuestsTargeted);
            AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
        }

        public bool On
        {
            get { return _on; }
            set {
                _on=value;
                Animator ani = GetComponent<Animator>();
                if (ani) ani.enabled = _on;
                Array.ForEach(SpotImages, s => {s.gameObject.SetActive(_on);});
            }
        }

        public GuestVO Guest
        {
            get { return _guest; }
        }

        private void HandleRemark(RemarkVO remark)
        {
            On = false;
            if (_guest != null && _guest != null && remark != null)
            {
                Color c;
                if (remark.Interest == _guest.Like) c = Colors.GetColor("Like");
                else if (remark.Interest == _guest.Dislike) c = Colors.GetColor("Dislike");
                else c = Colors.GetColor("Neutral");

                foreach (Image image in SpotImages)
                {
                    c.a = image.color.a;
                    image.color = c;
                }
            }
        }

        private void HandleGuestsTargeted(GuestVO [] guests)
        {
			On = guests != null
                && Array.IndexOf(guests, _guest) >= 0
                && !_intoxicated;
        }

        private void HandleGuests(GuestVO [] guests)
        {
            _guest = (guests != null && _index < guests.Length) ? guests[_index] : null;
        }

        private void HandleIntoxication(int tox)
		{
			_intoxicated = (tox >= 50);
		}
    }
}
