using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class CalendarLeaveEstateBtn : MonoBehaviour
    {
        public Button TabBtn;
        public Text ExitText;

        private bool _needsOutfit;

        public void OnClick()
        {
            if (_needsOutfit) TabBtn.onClick.Invoke();
            else AmbitionApp.SendMessage(EstateMessages.LEAVE_ESTATE);
        }

        private void OnEnable()
        {
            AmbitionApp.Calendar.Observe(HandleCalendarRefresh);
            AmbitionApp.Subscribe<PartyVO>(HandleParty);
            AmbitionApp.Subscribe<RendezVO>(HandleRendez);
        }

        private void OnDisable()
        {
            AmbitionApp.Calendar.Unobserve(HandleCalendarRefresh);
            AmbitionApp.Unsubscribe<PartyVO>(HandleParty);
            AmbitionApp.Unsubscribe<RendezVO>(HandleRendez);
        }

        private void HandleCalendarRefresh(CalendarModel calendar)
        {
            PartyVO[] parties = calendar.GetOccasions<PartyVO>();
            RendezVO[] liaisons = calendar.GetOccasions<RendezVO>();

            if (Array.Exists(parties, p => p.IsAttending))
            {
                ExitText.text = AmbitionApp.Localize(LocalizationConsts.EXIT_PARTY);
                _needsOutfit = true;
            }
            else if (Array.Exists(liaisons, r=>r.IsAttending))
            {
                ExitText.text = AmbitionApp.Localize(LocalizationConsts.EXIT_RENDEZVOUS);
                _needsOutfit = true;
            }
            else
            {
                ExitText.text = AmbitionApp.Localize(LocalizationConsts.EXIT_PARIS);
                _needsOutfit = false;
            }
        }

        private void HandleParty(PartyVO party)
        {
            if (party.Day == AmbitionApp.Calendar.Day)
            {
                HandleCalendarRefresh(AmbitionApp.Calendar);
            }
        }

        private void HandleRendez(RendezVO rendez)
        {
            if (rendez.Day == AmbitionApp.Calendar.Day)
            {
                HandleCalendarRefresh(AmbitionApp.Calendar);
            }
        }
    }
}
