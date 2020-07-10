using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class HeaderMediator : MonoBehaviour
    {
        public GameObject HeaderPrefab;

        // Unity does some wonky stuff with "null" when it comes to gameobjects, so track a flag
        private GameObject _instance = null;
        private bool _instantiated = false;

        void OnEnable()
        {
            AmbitionApp.Subscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Subscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        void OnDisable()
        {
            AmbitionApp.Unsubscribe(GameMessages.SHOW_HEADER, ShowHeader);
            AmbitionApp.Unsubscribe(GameMessages.HIDE_HEADER, HideHeader);
        }

        private void ShowHeader()
        {
            if (!_instantiated)
            {
                _instance = GameObject.Instantiate(HeaderPrefab, transform);
            }
            _instantiated = true;
        }

        private void HideHeader()
        {
            if (_instance != null) Destroy(_instance);
            _instantiated = false;
        }
    }
}
