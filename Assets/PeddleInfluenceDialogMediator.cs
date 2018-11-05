using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialog;

namespace Ambition
{
    public class PeddleInfluenceDialogMediator : DialogView, Util.IInitializable<Gossip>
    {
        public const string DIALOG_ID = "PEDDLE_INFLUENCE";
        public Text TitleText;
        public Text BodyText;
        public Text IncreaseStatText; //Text for the 'Use it to help this faction' button
        public Text DecreaseStatText; //Text for the 'Use it to harm this faction' button
        public Text DismissText; //Text for the cancel button
        private Gossip _gossip;
        FactionModel _factionModel = AmbitionApp.GetModel<FactionModel>();
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
            string phrase = "peddle_influence_dialog";

            //Setting up the dictionary for the necessary substitutions
            Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
            dialogSubstitutions.Add("$GOSSIPNAME", _gossip.Name());
            dialogSubstitutions.Add("$FACTION", _gossip.Faction);
            dialogSubstitutions.Add("$SHIFTAMOUNT", _gossip.PoliticalEffectValue().ToString());
            dialogSubstitutions.Add("$CAUGHTODDS", localizationModel.GetString("gossip_caught_odds." + (int)(Mathf.Clamp(_inventoryModel.GossipSoldOrPeddled, 0, 9))));

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

            if (IncreaseStatText != null)
            {
                if (_gossip.PowerShiftEffect())//If it's a power shift
                {
                    str = localizationModel.GetString(phrase + ".increase_power", dialogSubstitutions);
                    if (str != null && IncreaseStatText != null) IncreaseStatText.text = str;
                    else IncreaseStatText.text = localizationModel.GetString(DialogConsts.DEFAULT_CONFIRM);
                } else //If it's an allegiance shift
                {
                    str = localizationModel.GetString(phrase + ".increase_allegiance", dialogSubstitutions);
                    if (str != null && IncreaseStatText != null) IncreaseStatText.text = str;
                    else IncreaseStatText.text = localizationModel.GetString(DialogConsts.DEFAULT_CONFIRM);
                }
            }

            if (DecreaseStatText != null)
            {
                if (_gossip.PowerShiftEffect())//If it's a power shift
                {
                    str = localizationModel.GetString(phrase + ".decrease_power", dialogSubstitutions);
                    if (str != null && DecreaseStatText != null) DecreaseStatText.text = str;
                    else DecreaseStatText.text = localizationModel.GetString(DialogConsts.DEFAULT_CONFIRM);
                }
                else //If it's an allegiance shift
                {
                    str = localizationModel.GetString(phrase + ".decrease_allegiance", dialogSubstitutions);
                    if (str != null && DecreaseStatText != null) DecreaseStatText.text = str;
                    else DecreaseStatText.text = localizationModel.GetString(DialogConsts.DEFAULT_CONFIRM);
                }
            }
        }

        public void IncreaseFactionStat()
        {
            if (_gossip.PowerShiftEffect())//If it's a power shift
            {
                AmbitionApp.SendMessage(new AdjustFactionVO(_gossip.Faction, 0, _gossip.PoliticalEffectValue()));
            }
            else //If it's an allegiance shift
            {
                AmbitionApp.SendMessage(new AdjustFactionVO(_gossip.Faction, 0, 0, _gossip.PoliticalEffectValue()));
            }
            _inventoryModel.GossipItems.Remove(_gossip);
            _inventoryModel.GossipSoldOrPeddled++; //Adjust the amount of Gossip Items sold today
            AmbitionApp.SendMessage(InventoryMessages.PEDDLE_GOSSIP);
        }

        public void DecreaseFactionStat()
        {
            if (_gossip.PowerShiftEffect())//If it's a power shift
            {
                AmbitionApp.SendMessage(new AdjustFactionVO(_gossip.Faction, 0, _gossip.PoliticalEffectValue() * -1));
            }
            else //If it's an allegiance shift
            {
                AmbitionApp.SendMessage(new AdjustFactionVO(_gossip.Faction, 0, 0, _gossip.PoliticalEffectValue() * -1));
            }
            _inventoryModel.GossipItems.Remove(_gossip);
            _inventoryModel.GossipSoldOrPeddled++; //Adjust the amount of Gossip Items sold today
            AmbitionApp.SendMessage(InventoryMessages.PEDDLE_GOSSIP);
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
