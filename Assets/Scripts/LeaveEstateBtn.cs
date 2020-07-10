using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class LeaveEstateBtn : MonoBehaviour
	{
	    private Text _text;
        private DateTime _today;
        private PartyVO _party;

	    void Awake()
	    {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            _today = model.Date;
            _text = this.GetComponentInChildren<Text>();
            AmbitionApp.GetModel<PartyModel>().Observe<PartyModel>(HandleParty);
        }

        void OnDestroy() => AmbitionApp.GetModel<PartyModel>().Unobserve<PartyModel>(HandleParty);

        private void HandleParty(PartyModel model)
        {
            _party = model.GetParty();
            if (!_party?.Attending ?? false) _party = null;
            _text.text = AmbitionApp.Localize("calendar.btn." + (_party == null ? "paris" : "party"));
        }

        public void LeaveEstate() => AmbitionApp.SendMessage(EstateMessages.LEAVE_ESTATE);
    }
}
