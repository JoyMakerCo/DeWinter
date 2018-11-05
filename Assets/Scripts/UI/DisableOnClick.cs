using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    // Utility class. Prevents players from spamming the target button.
    public class DisableOnClick : MonoBehaviour
    {
        private Button _btn;
        // Use this for initialization
        void Awake()
        {
            _btn = gameObject.GetComponent<Button>();
            if (_btn != null) _btn.onClick.AddListener(DisableHandler);
        }

        private void OnEnable()
        {
            if (_btn != null) _btn.interactable = true;
        }

        private void OnDestroy()
        {
            if (_btn != null) _btn.onClick.RemoveListener(DisableHandler);
        }

        // Update is called once per frame
        private void DisableHandler()
        {
            _btn.interactable = false;
        }
    }
}
