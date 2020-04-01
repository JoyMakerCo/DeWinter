using Core;
namespace Ambition
{
    public class LoadLocationSceneCmd : ICommand
    {
        public void Execute()
        {
            ParisModel model = AmbitionApp.GetModel<ParisModel>();
            string location = model.Location?.ID;
            AmbitionApp.GetModel<LocalizationModel>().SetLocation(location);
            AmbitionApp.SendMessage(GameMessages.LOAD_SCENE, model.Location?.SceneID);
            AmbitionApp.SendMessage(GameMessages.SHOW_HEADER, ParisConsts.LOCALIZATION_PREFIX + location);
        }
    }
}
