using System;
using UFlow;
namespace Ambition
{
    public class MarketTutorialController : UFlowConfig
    {
        public override void Configure()
        {
            State("StartTutorial");
            State("SelectOutfit");
            State("SelectOutfitInput");
            State("OutfitStats");
            State("EndTutorialInput");
            State("ExitTutorial");

            Bind<FadeInInput>("StartTutorial");
            Bind<TutorialState>("SelectOutfit");
            Bind<EquipInput>("SelectOutfitInput");
            Bind<TutorialState>("OutfitStats");
            Bind<MessageInput, string>("EndTutorialInput", TutorialMessages.TUTORIAL_NEXT_STEP);
            Bind<EndTutorialState>("ExitTutorial");
        }
    }
    public class EquipInput : UInput, IDisposable
    {
        public EquipInput()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleEquip);
        }
        public void Dispose()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleEquip);
        }
        public void HandleEquip(ItemVO item) => Activate();
    }
}
