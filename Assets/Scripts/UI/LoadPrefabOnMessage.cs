using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class LoadPrefabOnMessage : MonoBehaviour
    {
        public string Message;
        public GameObject Prefab;

        // Start is called before the first frame update
        void Awake()
        {
            if (string.IsNullOrWhiteSpace(Message) || Prefab == null) GameObject.Destroy(gameObject);
            else AmbitionApp.Subscribe(Message, LoadPrefab);
        }

        // Update is called once per frame  
        private void LoadPrefab()
        {
            GameObject obj = GameObject.Instantiate(Prefab, transform.position, Quaternion.identity, transform.parent);
            obj.transform.SetSiblingIndex(transform.GetSiblingIndex());
            GameObject.Destroy(gameObject);
        }

        private void OnDestroy()
        {
            AmbitionApp.Unsubscribe(Message, LoadPrefab);
        }
    }
}
