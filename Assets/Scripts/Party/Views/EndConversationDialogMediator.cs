using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Core;

namespace Ambition
{
    public class EndConversationDialogMediator : DialogView, IPointerClickHandler
    {
        public const string DIALOG_ID = "END_CONVERSATION";

        public Text TitleText;
        public Text SubText;
        public CommodityTableView Commodities;

        private CharacterVO[] _guests;

        public override void OnOpen()
        {
            //RoomVO room = AmbitionApp.GetModel<MapModel>().Room;
            //_guests = room?.Guests;
            //Commodities.SetCommodities(room.Rewards);
            ////string phrase = Array.Exists(_guests, g => g.State == GuestState.Charmed) ? "success" : "failure";
            //SetPhrase("after_conversation_dialog.success");
        }
            
        protected void SetPhrase(string phrase)
        {
            TitleText.text = AmbitionApp.GetString(phrase + ".title");
            SubText.text = GetBodyText(phrase);
        }
    
        protected virtual string GetBodyText(string phrase)
        {
            int charmedTally = 0;
            int offendedTally = 0;
            foreach (CharacterVO g in _guests)
            {
                //if (g.State == GuestState.Charmed)
                //{
                //    charmedTally++;
                //}
                //else if (g.State == GuestState.Offended)
                //{
                //    offendedTally++;
                //}
            }
            if (offendedTally == 0)
            {
                return AmbitionApp.GetString(phrase + ".body_charmed_all");
            }
            else
            {
                Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
                dialogSubstitutions.Add("$CHARMEDAMOUNT", charmedTally.ToString());
                if (charmedTally == 1) dialogSubstitutions.Add("$CHARMEDSINGULARORPLURAL", AmbitionApp.GetString("guest_was"));
                else dialogSubstitutions.Add("$CHARMEDSINGULARORPLURAL", AmbitionApp.GetString("guests_were"));
                dialogSubstitutions.Add("$OFFENDEDAMOUNT", offendedTally.ToString());
                if (offendedTally == 1) dialogSubstitutions.Add("$OFFENDEDSINGULARORPLURAL", AmbitionApp.GetString("guest_was"));
                else dialogSubstitutions.Add("$OFFENDEDSINGULARORPLURAL", AmbitionApp.GetString("guests_were"));
                return AmbitionApp.GetString(phrase + ".body_charmed_some", dialogSubstitutions);
            }

        }

        public void OnPointerClick(PointerEventData data) => Close();
        override public void OnClose() => AmbitionApp.SendMessage(GameMessages.DIALOG_CLOSED, ID);
    }
}
