using System;
using UFlow;

namespace Ambition
{
    public class StyleChangeDecision : ULink
    {
        public override bool Validate()
        {
            GameModel game = AmbitionApp.GetModel<GameModel>();
            return game.Date >= game.NextStyleSwitchDay;
        }
    }
}
