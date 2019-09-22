using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class SellGossipDialogMediator : DialogView<ItemVO>
    {
        public const string DIALOG_ID = "SELL_GOSSIP";
        public Text TitleText;
        public Text BodyText;
        public Text SellText; //Text for the 'Yes, sell the gossip' button
        public Text DismissText; //Text for the cancel button
        private ItemVO _gossip;

        public override void OnOpen(ItemVO gossip)
        {
            _gossip = gossip;
            SetPhrase();
        }
               
        //This is how the localized text is put into all of the text boxes in the dialog
        public void SetPhrase()
        {
            System.DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
            string phrase = "sell_gossip_dialog";

            //Setting up the dictionary for the necessary substitutions
            Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
            dialogSubstitutions.Add("$GOSSIPNAME", _gossip.Name);
            dialogSubstitutions.Add("$GOSSIPPRICE", "£" + _gossip.Price.ToString("### ###"));
            //dialogSubstitutions.Add("$CAUGHTODDS", AmbitionApp.GetString("gossip_caught_odds." + (int)(Mathf.Clamp(AmbitionApp.GetModel<InventoryModel>().GossipSoldOrPeddled,0,9))));
            
            //Performing the substitutions themselves
            BodyText.text = AmbitionApp.GetString(phrase + DialogConsts.BODY, dialogSubstitutions);
            TitleText.text = AmbitionApp.GetString(phrase + DialogConsts.TITLE, dialogSubstitutions);
            string str;
            if (DismissText != null)
            {
                str = AmbitionApp.GetString(phrase + DialogConsts.CANCEL, dialogSubstitutions);
                if (str != null && DismissText != null) DismissText.text = str;
                else DismissText.text = AmbitionApp.GetString(DialogConsts.DEFAULT_CANCEL);
            }

            if (SellText != null)
            {
                str = AmbitionApp.GetString(phrase + DialogConsts.CONFIRM, dialogSubstitutions);
                if (str != null && SellText != null) SellText.text = str;
                else SellText.text = AmbitionApp.GetString(DialogConsts.DEFAULT_CONFIRM);
            }
        }

        public void SellGossip() => AmbitionApp.SendMessage(InventoryMessages.SELL_ITEM, _gossip);

        public override void OnOpen()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_OPENED, ID);
        }

        public override void OnClose()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
        }
    }
}
