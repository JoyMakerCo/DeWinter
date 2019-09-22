using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class PerilBarController : MonoBehaviour
    {
        public Image fillImage;
        private GameModel _Model => AmbitionApp.GetModel<GameModel>();

        void OnEnable() => _Model?.Peril.Observe(HandlePeril);
        void OnDisable() => _Model?.Peril.Remove(HandlePeril);
        void HandlePeril(int peril) => fillImage.fillAmount = peril * .01f;
    }
}
