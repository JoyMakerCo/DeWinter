using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class HeaderView : MonoBehaviour
    {
        public Text HeaderTitle;

        void OnEnable()
        {
            AmbitionApp.GetModel<LocalizationModel>().HeaderTitle.Observe(SetHeaderTitle);
        }

        private void OnDisable()
        {
            AmbitionApp.GetModel<LocalizationModel>().HeaderTitle.Remove(SetHeaderTitle);
        }

        private void SetHeaderTitle(string title) => HeaderTitle.text = title;
    }
}
