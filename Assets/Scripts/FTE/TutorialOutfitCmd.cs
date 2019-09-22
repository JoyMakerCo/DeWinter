using System;
using Core;

namespace Ambition
{
    public class TutorialOutfitCmd : ICommand
    {
        public void Execute()
        {
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialSellTabStep00");
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialSellingOutfitsStep01");
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialOutfitStatsStep02");
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialOutfitStyleStep03");
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialBuyTabStep04");
            AmbitionApp.RegisterState<TutorialState>("TutorialOutfitController", "TutorialBuyOutfitStep05");
        }
    }
}
