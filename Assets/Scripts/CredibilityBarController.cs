using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class CredibilityBarController : MonoBehaviour
    {
        public Image fillImage;
        private GameModel _model;

        void OnEnable ()
        {
            _model = AmbitionApp.GetModel<GameModel>();
            _model?.Credibility.Observe(HandleCredibility);
        }

        void OnDisable () => _model?.Credibility.Remove(HandleCredibility);
        private void HandleCredibility (int credibility) => fillImage.fillAmount = credibility * .01f;
    }
}
