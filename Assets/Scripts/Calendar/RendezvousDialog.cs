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
    public class RendezvousDialog : MonoBehaviour, Util.IInitializable<RendezVO>
    {
        public const string DIALOG_ID = "RENDEZVOUS_RSVP";
        public SpriteConfig InvitationConfig;

        public Text TitleTxt;
        public Text BodyTxt;
        public Text AcceptTxt;
        public Image Seal;
        public Image Stamp;
        public Image FavorBonus;
        public Image FavorPenalty;
        public GameObject AcceptBtn;
        public GameObject DeclineBtn;
        public Color AcceptColor;
        public Color DeclineColor;
        public GameObject FavorReward;
        public GameObject DeclinePenalty;

        private RendezVO _rendezvous;

        public void Initialize(RendezVO rendezvous)
        {
            CharacterVO character = AmbitionApp.GetModel<CharacterModel>().GetCharacter(rendezvous.Character);
            LocalizationModel localization = AmbitionApp.GetModel<LocalizationModel>();
            string faction = (character?.Faction ?? FactionType.None).ToString();
            string invitee = !rendezvous.IsCaller ? AmbitionApp.Localize(AmbitionApp.Game.playerID + ".shortname")
                : (character?.Formal ?? false) ? localization.GetFormalName(character, rendezvous.Character)
                : localization.GetShortName(character, rendezvous.Character);

            Dictionary<string, string> subs = new Dictionary<string, string>()
            {
                {"$NAME", invitee},
                {"$RENDEZVOUSDATE", AmbitionApp.GetModel<LocalizationModel>().Localize(AmbitionApp.Calendar.StartDate.AddDays(rendezvous.Day))},
                {"$RENDEZVOUSLOCATION", AmbitionApp.Localize(ParisConsts.LABEL + rendezvous.Location)}
            };
            string characterID = rendezvous.IsCaller ? AmbitionApp.Game.playerID : rendezvous.Character;
            _rendezvous = rendezvous;

            // To force it to work with the RSVP animation, the title is the letter and the body is the signature
            TitleTxt.text = AmbitionApp.Localize("rendezvous." + characterID.ToLower() + ".rsvp.body", subs);
            BodyTxt.text = _rendezvous.IsCaller ? AmbitionApp.Localize(AmbitionApp.Game.playerID + ".name")
                : (character?.Formal ?? false) ? localization.GetFormalName(character, rendezvous.Character)
                : localization.GetShortName(character, rendezvous.Character);
            Seal.sprite = InvitationConfig.GetSprite("seal." + faction);
            Stamp.sprite = InvitationConfig.GetSprite(faction);
            FavorBonus.sprite = InvitationConfig.GetSprite(faction);
            FavorPenalty.sprite = InvitationConfig.GetSprite(faction);
            FavorReward.SetActive(!_rendezvous.IsCaller && AmbitionApp.Calendar.Day < rendezvous.Created + 1); // Prompt response bonus
            SetText(rendezvous.RSVP, _rendezvous.IsCaller);
            DeclinePenalty?.SetActive(!_rendezvous.IsCaller);
        }

        public void Accept()
        {
            AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, _rendezvous);
            SetText(RSVP.Accepted, _rendezvous.IsCaller);
        }

        public void Decline()
        {
            AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, _rendezvous);
            SetText(RSVP.Declined, _rendezvous.IsCaller);
        }

        private void Awake()
        {
            AmbitionApp.Subscribe<RendezVO>(PartyMessages.DECLINE_INVITATION, OnDecline);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<RendezVO>(PartyMessages.DECLINE_INVITATION, OnDecline);
        }

        private void SetText(RSVP rsvp, bool isCaller)
        {
            bool showButtons = !isCaller && rsvp == RSVP.New;
            switch(rsvp)
            {
                case RSVP.Accepted:
                    AcceptTxt.text = AmbitionApp.Localize("rsvp.accepted");
                    AcceptTxt.color = AcceptColor;
                    break;
                case RSVP.Declined:
                    AcceptTxt.text = AmbitionApp.Localize("rsvp.declined");
                    AcceptTxt.color = DeclineColor;
                    break;
                default:
                    AcceptTxt.text = AmbitionApp.Localize("calendar.eventlist.pending");
                    break;
            }
            AcceptTxt.enabled = !showButtons;
            AcceptBtn.SetActive(showButtons);
            DeclineBtn.SetActive(showButtons);
        }

        private void OnDecline(RendezVO rendez)
        {
            if (rendez == _rendezvous)
            {
                SetText(_rendezvous.RSVP, rendez.IsCaller);
            }
        }
    }
}
