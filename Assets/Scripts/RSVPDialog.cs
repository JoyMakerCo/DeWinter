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
    public class RSVPDialog : DialogView<PartyVO>
    {
        public const string DIALOG_ID = "RSVP";
        public SpriteConfig InvitationConfig;

        public Text TitleTxt;
        public Text BodyTxt;
        public Text ObjectiveText;
        public Text HostText;
        public Image Seal;
        public Image Stamp;

        private PartyVO _party;

        public override void OnOpen(PartyVO party)
        {
            ServantModel smod = AmbitionApp.GetModel<ServantModel>();
            Dictionary<string, string> dialogsubs = new Dictionary<string, string>(){
                {"$PARTYSIZE", AmbitionApp.Localize("party_importance." + ((int)party.Size).ToString())}};

            _party = party;
            TitleTxt.text = party.Host;
/*
            if (smod.Servants.ContainsKey(ServantConsts.SPYMASTER))
            {
                if (_party.Enemies != null && _party.Enemies.Length > 0)
                {
                    string enemyList = "";
                    Dictionary<string, string> subs = new Dictionary<string, string>();
                    foreach (EnemyVO enemy in _party.Enemies)
                    {
                        enemyList += "\n" + enemy.Name;
                    }
                    subs.Add("$ENEMYLIST", enemyList);
                    dialogsubs.Add("$PROMPT", AmbitionApp.GetString("party_enemies", subs));
                }
                else
                {
                    dialogsubs.Add("$PROMPT", AmbitionApp.GetString("party_no_enemies"));
                }
            }
            else
            {
                dialogsubs.Add("$PROMPT", AmbitionApp.GetString("party_prompt"));
            }
*/
            ObjectiveText.text = AmbitionApp.Localize("party_objectives");
            HostText.text = AmbitionApp.Localize("rsvp");
            Seal.sprite = InvitationConfig.GetSprite("seal." + party.Faction);
            Stamp.sprite = InvitationConfig.GetSprite(party.Faction.ToString());
            BodyTxt.text = party.Invitation;
        }

        public void RSVPAccept() => AmbitionApp.SendMessage(PartyMessages.ACCEPT_INVITATION, _party);
        public void RSVPDecline() => AmbitionApp.SendMessage(PartyMessages.DECLINE_INVITATION, _party);
    }
}
