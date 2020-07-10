using Core;
namespace Ambition
{
    public class LoadLocationLink : LoadSceneLink, Util.IInitializable
    {
        public void Initialize() => Initialize(AmbitionApp.GetModel<ParisModel>().Location?.SceneID);
    }
}
