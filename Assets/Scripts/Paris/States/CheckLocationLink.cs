using UFlow;
namespace Ambition
{
    public class CheckLocationLink : ULink
    {
<<<<<<< Updated upstream:Assets/Scripts/Calendar/Commands/CheckLocationLink.cs
        override public bool Validate() => AmbitionApp.GetModel<ParisModel>().Location != null;
=======
        public override bool Validate()
        {
            ParisModel paris = AmbitionApp.GetModel<ParisModel>();
            return !string.IsNullOrEmpty(paris.LocationID ?? paris.Location?.ID);
        }
>>>>>>> Stashed changes:Assets/Scripts/Paris/States/CheckLocationLink.cs
    }
}
