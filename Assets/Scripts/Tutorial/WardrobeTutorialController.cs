using System;
using UFlow;
namespace Ambition
{
    public class WardrobeTutorialController : UFlow.UFlowConfig
    {
        public const string CONTROLLER_ID = "WardrobeTutorialController";
        public override void Configure()
        {
            State("StartTutorialStep");
            State("ExitCalendarStep");
            State("ExitCalendarInput");
            State("SelectOutfitStep");
            State("SelectOutfitInput");
            State("GoToPartyStep");
            State("GoToPartyInput");
            State("EndTutorialStep");

            Bind<TutorialState>("ExitCalendarStep");
            Bind<TutorialState>("SelectOutfitStep");
            Bind<TutorialState>("GoToPartyStep");
            Bind<EndTutorialState>("EndTutorialStep");

            Bind<MessageInput, string>("ExitCalendarInput", TutorialMessages.TUTORIAL_NEXT_STEP);
            Bind<EquipOutfitTutorialInput>("SelectOutfitInput");
            Bind<MessageInput, string>("GoToPartyInput", EstateMessages.LEAVE_ESTATE);
        }
    }

    public class EquipOutfitTutorialInput : UInput, Util.IInitializable, IDisposable
    {
        public void Initialize()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
        }

        public void Dispose()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
        }

        private void HandleEquip(ItemVO item)
        {
            if (item != null) Activate();
        }
    }
}
