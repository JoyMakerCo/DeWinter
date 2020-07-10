using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Ambition
{
    public class StreamRequester : MonoBehaviour
    {
        private Dictionary<string, Delegate> _handlers = new Dictionary<string, Delegate>();

        public void Request<T>(string url, Action<T> action)
        {
            StartCoroutine(GetRequest<T>(url, action));
        }

        IEnumerator GetRequest<T>(string url, Action<T> action)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.Send();

#if DEBUG
            if (www.isNetworkError) Debug.Log(www.error);
#endif
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
