using System;
using UFlow;
namespace Ambition
{
    public class CheckChapterLink : ULink
    {
        public override bool Validate()
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            return Array.Exists(model.Chapters, c => c.Date == model.Date);
        }
    }
}
