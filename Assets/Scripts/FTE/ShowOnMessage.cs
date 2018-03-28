using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
    public class ShowOnMessage : MonoBehaviour
    {
        public string MessageID;
        public bool OneShot=true;
        void Awake()
        {
            AmbitionApp.Subscribe(MessageID, HandleMessage);
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(MessageID, HandleMessage);
        }

        private void HandleMessage()
        {
            this.gameObject.SetActive(true);
            if (OneShot) Destroy(this);
        }
    }
}
