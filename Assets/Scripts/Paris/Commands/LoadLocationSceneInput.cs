using Core;
using UFlow;
namespace Ambition
{
    public class LoadLocationSceneInput : LoadSceneInput
    {
        public LoadLocationSceneInput() : base()
        {
            base.Initialize(AmbitionApp.Paris.GetLocation()?.SceneID);
        }
    }
}
