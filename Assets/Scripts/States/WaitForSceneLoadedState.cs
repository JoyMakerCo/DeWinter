using UFlow;

namespace Ambition
{
    public class WaitForSceneLoadedState : AmbitionValueLink<string>
    {
        override public void Initialize()
        {
            MessageID = GameMessages.SCENE_LOADED;
            ValidateOnCallback = m=>{ return m == Data; };
            base.Initialize();
        }
    }
}
