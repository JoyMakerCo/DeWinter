using Core;
namespace Ambition
{
    public class LoadLocationSceneCmd : ICommand
    {
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, model.Location?.Scene);
        }
    }
}
