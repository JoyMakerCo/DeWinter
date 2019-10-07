using System;
namespace Ambition
{
    public class CheckLocationSceneLink : UFlow.ULink
    {
        public override bool Validate() => !string.IsNullOrWhiteSpace(AmbitionApp.GetModel<ParisModel>()?.Location?.SceneID);
    }
}
