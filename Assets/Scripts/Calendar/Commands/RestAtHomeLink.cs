using System;
namespace Ambition
{
    public class RestAtHomeLink : UFlow.ULink
    {
        public override bool Validate() => AmbitionApp.GetModel<ParisModel>().Location?.ID == ParisConsts.HOME;
    }
}
