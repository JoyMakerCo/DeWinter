using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class ParisView : MonoBehaviour
    {
        ParisModel _paris;

        private void Start()
        {
            LocationPin[] pins = transform.GetComponentsInChildren<LocationPin>(true);
            LocationVO location;
            _paris = AmbitionApp.GetModel<ParisModel>();
            foreach(LocationPin pin in pins)
            {
                location = pin.GetLocation();
                pin.gameObject.SetActive(_paris.Locations.ContainsKey(pin.name));
                if (pin.isActiveAndEnabled)
                {
                    _paris.Locations[location.Name] = location;
                }
                //else if (CheckRequirements(location.Requirements))
                //{
                //    SendMessage(ParisMessages.ADD_LOCATION, location.Name);
                //    pin.gameObject.SetActive(true);
                //    _paris.Locations[location.Name] = location;
                //}
            }
        }
    }
}
