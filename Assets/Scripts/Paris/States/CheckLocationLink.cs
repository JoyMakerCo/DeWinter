using UFlow;
namespace Ambition
{
    public class CheckLocationLink : ULink
    {
        public override bool Validate() => !string.IsNullOrEmpty(AmbitionApp.GetModel<ParisModel>().Location?.ID);
    }
}
