using System;
namespace Ambition
{
    public class CredibilityBarController : HeaderBarController
    {
        protected override string GetMessageID() => GameConsts.CREDIBILITY;
        protected override int GetInitialStat() => AmbitionApp.Game.Credibility;
    }
}
