using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class OpenDialogBehaviour : MonoBehaviour
    {
        public void OpenDialog(string dialogID) => AmbitionApp.OpenDialog(dialogID);
    }
}
