using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class VersionTxt : MonoBehaviour
    {
        public Text VersionText;

        private void Start()
        {
            VersionText.text = AmbitionApp.Localize("version") + " " + Application.version;
        }
    }
}
