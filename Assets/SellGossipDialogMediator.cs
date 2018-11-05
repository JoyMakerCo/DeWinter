using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class SellGossipDialogMediator : DialogView, Util.IInitializable<Gossip>
    {
        public const string DIALOG_ID = "SELL_GOSSIP";
        public Text TitleText;
        public Text BodyText;
        public Text SellText; //Text for the 'Yes, sell the gossip' button
        public Text DismissText; //Text for the cancel button
        private Gossip _gossip;
        GameModel _gameModel = AmbitionApp.GetModel<GameModel>();
        InventoryModel _inventoryModel = AmbitionApp.GetModel<InventoryModel>();

        public void Initialize(Gossip gossip)
        {
            _gossip = gossip;
            SetPhrase();
        }
               
        //This is how the localized text is put into all of the text boxes in the dialog
        public void SetPhrase()
        {
            Core.LocalizationModel localizationModel = AmbitionApp.GetModel<Core.LocalizationModel>();
            string phrase = "sell_gossip_dialog";

            //Setting up the dictionary for the necessary substitutions
            Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
            dialogSubstitutions.Add("$GOSSIPNAME", _gossip.Name());
            dialogSubstitutions.Add("$GOSSIPPRICE", "£" + _gossip.LivreValue().ToString("### ###"));
            dialogSubstitutions.Add("$CAUGHTODDS", localizationModel.GetString("gossip_caught_odds." + (int)(Mathf.Clamp(_inventoryModel.GossipSoldOrPeddled,0,9))));
            
            //Performing the substitutions themselves
            BodyText.text = localizationModel.GetString(phrase + DialogConsts.BODY, dialogSubstitutions);
            TitleText.text = localizationModel.GetString(phrase + DialogConsts.TITLE, dialogSubstitutions);
            string str;
            if (DismissText != null)
            {
                str = localizationModel.GetString(phrase + DialogConsts.CANCEL, dialogSubstitutions);
                if (str != null && DismissText != null) DismissText.text = str;
                else DismissText.text = localizationModel.GetString(DialogConsts.DEFAULT_CANCEL);
            }

            if (SellText != null)
            {
                str = localizationModel.GetString(phrase + DialogConsts.CONFIRM, dialogSubstitutions);
                if (str != null && SellText != null) SellText.text = str;
                else SellText.text = localizationModel.GetString(DialogConsts.DEFAULT_CONFIRM);
            }
        }

        public void SellGossip()
        {
            _gameModel.Livre += _gossip.LivreValue();
            _inventoryModel.GossipItems.Remove(_gossip);
            _inventoryModel.GossipSoldOrPeddled++; //Adjust the amount of Gossip Items sold today
            AmbitionApp.SendMessage(InventoryMessages.SELL_GOSSIP);
        }

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
