using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
    public class RangeWarning : MonoBehaviour
    {
        public string ValueID;
        public Vector2Int Range;
        public bool OneShot=true;
        void Awake()
        {
            AmbitionApp.Subscribe<int>(ValueID, HandleValue);
            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<int>(ValueID, HandleValue);
        }

        private void HandleValue(int value)
        {
            this.gameObject.SetActive((value < Range.x || value > Range.y));
            if (this.gameObject.activeSelf && OneShot) Destroy(this);
        }
    }
}
