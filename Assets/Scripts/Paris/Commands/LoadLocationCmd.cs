using Core;
namespace Ambition
{
    public class LoadLocationCmd : ICommand
    {
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            if (model.Location != null && model.Location.Scene != null)
            {
                AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, model.Location.Scene);
            }
        }
    }
}
