using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Ambition;
using Dialog;
using Core;

namespace Ambition
{
    public class RSVPDialog : MonoBehaviour, Util.IInitializable<PartyVO>
    {
        public SpriteConfig InvitationConfig;

        public Text TitleTxt;
        public Text BodyTxt;
        public Text AcceptedTxt;
        public Image Seal;
        public Image Stamp;
        public GameObject AcceptBtn;
        public GameObject DeclineBtn;

        public GameObject CredReward;
        public Text CredRewardTxt;
        public Text CredPenaltyTxt;
        public Text GossipRewardTxt;
        public Image GossipImage;
        public Color AcceptColor;
        public Color DeclineColor;

        private PartyVO _party;

        public void Initialize(PartyVO party)
        {
            PartyModel model = AmbitionApp.GetModel<PartyModel>();
            Dictionary<string, string> subs = new Dictionary<string, string>();
            CharacterVO host = AmbitionApp.GetModel<CharacterModel>().GetCharacter(party.Host);
            _party = party;

            TitleTxt.text = AmbitionApp.Localization.GetFormalName(host, party.Host);
            SetRSVPText(party.RSVP);
            GossipImage.sprite = Stamp.sprite = InvitationConfig.GetSprite(party.Faction.ToString());
            BodyTxt.text = AmbitionApp.Localization.GetPartyInvitation(party, host);
            GossipRewardTxt.text = AmbitionApp.Localize("rsvp.gossip.reward", new Dictionary<string, string>() { { "%f", AmbitionApp.Localize(party.Faction.ToString().ToLower()) } });
            CredRewardTxt.text = AmbitionApp.Localize("rsvp.credibility.bonus", new Dictionary<string, string>() { { "%n", model.AcceptInvitationBonus.ToString() } });
            CredPenaltyTxt.text = AmbitionApp.Localize("rsvp.credibility.penalty", new Dictionary<string, string>() { { "%n", model.IgnoreInvitationPenalty.ToString() } });
            CredReward.SetActive(AmbitionApp.Calendar.Day == party.Created);
            Seal.sprite = InvitationConfig.GetSprite("seal." + party.Faction.ToString().ToLower());
        }

        public void RSVPAccept()
        {
            AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, _party);
            SetRSVPText(RSVP.Accepted);
        }

        public void RSVPDecline()
        {
            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, _party);
            SetRSVPText(RSVP.Declined);
        }

        private void SetRSVPText(RSVP rsvp)
        {
            string txt = rsvp == RSVP.Declined ? "rsvp.declined" : "rsvp.accepted";
            bool isNew = rsvp == RSVP.New;
            AcceptedTxt.text = AmbitionApp.Localize(txt);
            AcceptedTxt.color = rsvp == RSVP.Declined ? DeclineColor : AcceptColor;
            AcceptedTxt.enabled = !isNew;
            AcceptBtn.SetActive(isNew);
            DeclineBtn.SetActive(isNew);
        }

        private void Awake()
        {
            AmbitionApp.Subscribe<PartyVO>(PartyMessages.DECLINE_INVITATION, HandleDecline);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.DECLINE_INVITATION, HandleDecline);
        }

        private void HandleDecline(PartyVO party) => SetRSVPText(_party.RSVP);
    }
}
