using UFlow;

namespace Ambition
{
    public class WaitForSceneLoadedLink : AmbitionValueLink<string>
    {
        public override void SetValue(string data)
        {
            ValueID = GameMessages.SCENE_LOADED;
            ValidateOnInit = true;
            ValidateOnCallback = true;
            Value = data;
        }

        override protected string GetValue()
        {
            string scene = AmbitionApp.GetModel<GameModel>().Scene;
            return scene != null ? scene : Value;
        }
    }
}
