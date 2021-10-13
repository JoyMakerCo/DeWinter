using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class LoadOutView : MonoBehaviour
    {
        private const string STATUS_EFFECT_LOC = "estate.wardrobe.status_effect";
        private const string WELL_RESTED_LOC = "estate.wardrobe.player_status.well_rested";
        private const string EXHAUSTED_LOC = "estate.wardrobe.player_status.exhausted";
        private const string EFFECT_TOKEN = "%n";

        public GameObject LoadOutGameObject;
        public Button LoadOutButton;
        public Text ButtonText;
        public SpriteConfig FactionSymbols;
        public Text NameText;
        public Text DescriptionText;
        public Image FactionSymbol;
        public GameObject PlayerStatus;
        public Text ExhaustionLabel;
        public Text ExhaustionValue;
        public Image ExhaustionIcon;
        public Sprite[] ExhaustionIcons;
        public Color ExhaustionPenaltyColor = Color.red;
        public Color RestedBonusColor = Color.green;
        Dictionary<string, string> subs = new Dictionary<string, string>();
        public GameObject TooltipObject;
        public Text TooltipText;

        public void ShowTooltip(bool show)
        {
            TooltipObject.SetActive(show && !string.IsNullOrEmpty(TooltipText.text));
        }

        private void OnEnable()
        {
            PartyModel party = AmbitionApp.GetModel<PartyModel>();
            CharacterModel characters = AmbitionApp.GetModel<CharacterModel>();
            CalendarEvent cEvent = party.Party as CalendarEvent ?? characters.Rendezvous as CalendarEvent;
            FactionType faction = FactionType.None;
            LocalizationModel loc = AmbitionApp.GetModel<LocalizationModel>();
            int exhaustion = AmbitionApp.Game.Exhaustion;
            int[] penalties = AmbitionApp.Game.ExhaustionPenalties;
            int exhaustionPenalty;

            LoadOutGameObject.SetActive(cEvent != null);

            if (cEvent is PartyVO)
            {
                string str = AmbitionApp.Localize(PartyConstants.PARTY_DESCRIPTION + cEvent.ID);
                PartyVO pty = (PartyVO)cEvent;
                CharacterVO host = characters.GetCharacter(pty.Host);
                NameText.text = loc.GetPartyName(pty);
                ButtonText.text = AmbitionApp.Localize("calendar.btn.party");
                faction = (cEvent as PartyVO).Faction;
                DescriptionText.text = string.IsNullOrEmpty(str)
                    ? loc.GetFormalName(host, pty.Host) + " " + loc.GetPartyInvitation(pty, host)
                    : str;
                TooltipText.text = AmbitionApp.Localize("party_" + faction.ToString().ToLower() + "_likes_and_dislikes");
            }
            else if (cEvent is RendezVO)
            {
                CharacterVO character = characters.GetCharacter(((RendezVO)cEvent).Character);
                faction = character?.Faction ?? FactionType.None;
                NameText.text = AmbitionApp.Localize(CharacterConsts.LOC_NAME + cEvent.ID);
                ButtonText.text = AmbitionApp.Localize("calendar.btn.rendezvous");
                DescriptionText.text = AmbitionApp.Localize(ParisConsts.DESCRIPTION + (cEvent as RendezVO).Location);
                TooltipText.text = AmbitionApp.Localize("rendezvous_" + faction.ToString().ToLower() + "_likes_and_dislikes");
            }
            else TooltipText.text = null;

            FactionSymbol.enabled = faction != FactionType.None;
            PlayerStatus.gameObject.SetActive(exhaustion != 0);
            ExhaustionLabel.text = AmbitionApp.Localize(exhaustion > 0 ? EXHAUSTED_LOC : WELL_RESTED_LOC);
            exhaustionPenalty = exhaustion < 0
                ? AmbitionApp.Game.WellRestedBonus
                : exhaustion >= penalties.Length
                ? penalties[penalties.Length - 1]
                : penalties[exhaustion];
            subs[EFFECT_TOKEN] = exhaustionPenalty > 0 ? (" + " + exhaustionPenalty) : exhaustionPenalty.ToString();
            ExhaustionValue.text = AmbitionApp.Localize(STATUS_EFFECT_LOC, subs);
            FactionSymbol.sprite = FactionSymbols.GetSprite(faction.ToString());
            ExhaustionIcon.sprite = exhaustion < 0
                ? ExhaustionIcons[0]
                : exhaustion < ExhaustionIcons.Length
                ? ExhaustionIcons[exhaustion]
                : ExhaustionIcons[ExhaustionIcons.Length];
            ExhaustionValue.color = exhaustion < 0 ? RestedBonusColor : ExhaustionPenaltyColor;
        }

        private void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleBrowse);
        }

        private void HandleBrowse(ItemVO item)
        {
            LoadOutButton.interactable = item is OutfitVO;
            AmbitionApp.SendMessage(InventoryMessages.EQUIP, item);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleBrowse);
        }
    }
}
