using System;
namespace Ambition
{
    public class UpdateExhaustionCmd : Core.ICommand
    {
        public void Execute()
        {
            GameModel model = AmbitionApp.GetModel<GameModel>();
            model.Exhaustion.Value = model.Exhaustion > 0 ? 0 : -1;
        }
    }
}
