using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Dialog;

namespace Ambition
{
    public class QuestFulfilledDialogMediator : DialogView<ItemVO>
    {
        public Text TitleText;
        public Text BodyText;
        public Text OkText; //Text for the 'Yes, sell the gossip' button

        //This is how the localized text is put into all of the text boxes in the dialog
        public void SetPhrase()
        {
            System.DateTime today = AmbitionApp.GetModel<CalendarModel>().Today;
            string phrase = DialogConsts.REDEEM_QUEST_DIALOG;

            //Setting up the dictionary for the necessary substitutions
            Dictionary<string, string> dialogSubstitutions = new Dictionary<string, string>();
            
            //Performing the substitutions themselves
            BodyText.text = AmbitionApp.GetString(phrase + DialogConsts.BODY, dialogSubstitutions);
            TitleText.text = AmbitionApp.GetString(phrase + DialogConsts.TITLE, dialogSubstitutions);
            if (OkText != null)
            {
                OkText.text = AmbitionApp.Localize(DialogConsts.DEFAULT_CONFIRM);
            }
        }

        public override void OnOpen()
        {
			SetPhrase();
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_OPENED, ID);
        }

        public override void OnClose()
        {
            AmbitionApp.SendMessage<string>(GameMessages.DIALOG_CLOSED, ID);
        }

		public void DeliverReward()
		{
			AmbitionApp.GetModel<QuestModel>().CompleteQuest();
		}
    }
}
