using System;
namespace Ambition
{
    public class FleeConversationTutorialDialog : EndConversationDialogMediator
    {
        public new const string DIALOG_ID = "FLEE_CONVERSATION_TUTORIAL";
        public override void OnOpen()
        {
            AmbitionApp.Subscribe<CommodityVO[]>(PartyMessages.FLEE_PENALTIES, HandleFleePenalties);
            SetPhrase("out_of_confidence_tutorial_dialog");
        }

        public override void OnClose()
        {
            AmbitionApp.Unsubscribe<CommodityVO[]>(PartyMessages.FLEE_PENALTIES, HandleFleePenalties);
            base.OnClose();
        }

        private void HandleFleePenalties(CommodityVO[] penalties)
        {
            Commodities.SetCommodities(penalties);
        }

        protected override string GetBodyText(string phrase) => phrase + ".body";
    }
}
