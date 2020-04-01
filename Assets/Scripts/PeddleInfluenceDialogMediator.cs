using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;

namespace Ambition
{
    public class PeddleInfluenceDialogMediator : DialogView<ItemVO>
    {
        public const string DIALOG_ID = "PEDDLE_INFLUENCE";
        public Text TitleText;
        public Text BodyText;
        public Text IncreaseStatText; //Text for the 'Use it to help this faction' button
        public Text DecreaseStatText; //Text for the 'Use it to harm this faction' button
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
            Debug.Log("PeddleInfluenceDialogMediator.SetPhrase");
            //InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();
            string phrase = "peddle_influence_dialog";
            int shiftValue = GossipWrapperVO.GetEffect(_gossip);
            string locShift = AmbitionApp.Localize("shift_degree." + shiftValue.ToString() );
            ////Setting up the dictionary for the necessary substitutions
            Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
            dialogSubstitutions.Add("$SHIFTAMOUNT", locShift);
            dialogSubstitutions.Add("$GOSSIPNAME", _gossip.Name);
            dialogSubstitutions.Add("$GOSSIPPRICE", "£ " + GossipWrapperVO.GetValue(_gossip).ToString("### ###"));
            var activity = Mathf.Clamp(AmbitionApp.GetModel<InventoryModel>().GossipActivity,0,9);
            dialogSubstitutions.Add("$CAUGHTODDS", AmbitionApp.Localize("gossip_caught_odds." + activity.ToString() ) );
            // GossipWrapperVO.GetFaction(_gossip) would just round trip the ID to the enum and back...
            dialogSubstitutions.Add("$FACTION", AmbitionApp.Localize( _gossip.ID.ToLower() ) ); 
            //Performing the substitutions themselves
            BodyText.text = AmbitionApp.GetString(phrase + DialogConsts.BODY, dialogSubstitutions);
            TitleText.text = AmbitionApp.GetString(phrase + DialogConsts.TITLE, dialogSubstitutions);
            string str;
            if (DismissText != null)
            {
                str = AmbitionApp.GetString(phrase + DialogConsts.CANCEL, dialogSubstitutions);
                if (str != null && DismissText != null) DismissText.text = str;
                else DismissText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CANCEL);
            }

            if (IncreaseStatText != null)
            {
                if (GossipWrapperVO.IsPowerShift(_gossip))
                {
                    str = AmbitionApp.GetString(phrase + ".increase_power", dialogSubstitutions);
                    if (str != null && IncreaseStatText != null) IncreaseStatText.text = str;
                    else IncreaseStatText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                } else // it's an allegiance shift
                {
                    str = AmbitionApp.GetString(phrase + ".increase_allegiance", dialogSubstitutions);
                    if (str != null && IncreaseStatText != null) IncreaseStatText.text = str;
                    else IncreaseStatText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                }
            }

            if (DecreaseStatText != null)
            {
                if (GossipWrapperVO.IsPowerShift(_gossip))
                {
                    str = AmbitionApp.GetString(phrase + ".decrease_power", dialogSubstitutions);
                    if (str != null && DecreaseStatText != null) DecreaseStatText.text = str;
                    else DecreaseStatText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                }
                else // it's an allegiance shift
                {
                    str = AmbitionApp.GetString(phrase + ".decrease_allegiance", dialogSubstitutions);
                    if (str != null && DecreaseStatText != null) DecreaseStatText.text = str;
                    else DecreaseStatText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
                }
            }
        }

        public void IncreaseFactionStat()
        {
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();

            int shiftValue = GossipWrapperVO.GetEffect(_gossip);
            if (GossipWrapperVO.IsPowerShift(_gossip))
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakePowerVO( GossipWrapperVO.GetFaction(_gossip), shiftValue));
            }
            else // it's an allegiance shift
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakeAllegianceVO( GossipWrapperVO.GetFaction(_gossip), shiftValue));
            }

            AmbitionApp.SendMessage(InventoryMessages.PEDDLE_GOSSIP,_gossip);            
        }

        public void DecreaseFactionStat()
        {
            int shiftValue = GossipWrapperVO.GetEffect(_gossip);
            InventoryModel inventory = AmbitionApp.GetModel<InventoryModel>();

            if (GossipWrapperVO.IsPowerShift(_gossip))
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakePowerVO( GossipWrapperVO.GetFaction(_gossip), -shiftValue));
            }
            else // it's an allegiance shift
            {
                AmbitionApp.SendMessage(FactionMessages.ADJUST_FACTION,AdjustFactionVO.MakeAllegianceVO( GossipWrapperVO.GetFaction(_gossip), -shiftValue));
            }
            AmbitionApp.SendMessage(InventoryMessages.PEDDLE_GOSSIP,_gossip);
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
