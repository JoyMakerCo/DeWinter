using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SpotlightView : GuestViewMediator
    {
        public Util.ColorConfig Colors;
        public Image[] SpotImages;
        private bool _intoxicated;
        private bool _on;

        private void Awake() => InitGuest();
        private void OnDestroy() => Cleanup();

        private void OnEnable()
        {

            AmbitionApp.Subscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuestTargeted);
            AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleIntoxication);
            On = false;
        }

        private void OnDisable()
        {
            AmbitionApp.Unsubscribe<RemarkVO>(HandleRemark);
            AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_TARGETED, HandleGuestTargeted);
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

        private void HandleRemark(RemarkVO remark)
        {
            On = false;
            if (_guest != null && remark != null)
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

        private void HandleGuestTargeted(GuestVO guest)
        {
            if (guest == null) On = false;
            else if (guest == _guest) On = !_intoxicated;
        }

        private void HandleIntoxication(int tox)
		{
			_intoxicated = (tox >= 50);
		}

        protected override void HandleGuest(GuestVO guest) {}
    }
}
