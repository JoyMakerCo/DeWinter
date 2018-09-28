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
    public class RSVPDialog : DialogView, Util.IInitializable<PartyVO>
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

        public void Initialize(PartyVO party)
        {
            ServantModel smod = AmbitionApp.GetModel<ServantModel>();
            Dictionary<string, string> dialogsubs = new Dictionary<string, string>(){
                {"$PARTYSIZE", AmbitionApp.GetString("party_importance." + ((int)party.Importance).ToString())}};

            _party = party;
            TitleTxt.text = party.Host;

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

            ObjectiveText.text = AmbitionApp.GetString("party_objectives");
            HostText.text = AmbitionApp.GetString("rsvp");
            Seal.sprite = InvitationConfig.GetSprite("seal." + party.Faction.ToLower());
            Stamp.sprite = InvitationConfig.GetSprite(party.Faction.ToLower());
            BodyTxt.text = party.Invitiation;
        }

        public void RSVPAction(int decision)
        {
            _party.RSVP = (RSVP)decision;
            AmbitionApp.SendMessage(_party);
        }
    }
}
