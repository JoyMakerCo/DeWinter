using System;
namespace Ambition
{
    public class PerilBarController : HeaderBarController
    {
        protected override string GetMessageID() => GameConsts.PERIL;
        protected override int GetInitialStat() => AmbitionApp.Game.Peril;
    }
}
