using System.Collections;
using UnityEngine;

namespace Ambition
{
    public class TutorialBounce : MonoBehaviour
    {
        private const float BOUNCE_TIME = 1f;
     
        private Vector3 _pos;
        void OnEnable()
        {
            _pos = transform.localPosition;
            StartCoroutine(BounceCR());
        }

        void OnDisable()
        {
            StopAllCoroutines();
            transform.localPosition = _pos;
        }

        IEnumerator BounceCR()
        {
            float time = 0f;
            Vector3 pos = _pos;
            float bounce = Mathf.PI * 2f / BOUNCE_TIME;
            float a = gameObject.GetComponent<RectTransform>().rect.height*transform.localScale.y*0.5f;
            while (true)
            {
                pos.y = _pos.y + a*(Mathf.Abs(Mathf.Sin(time*bounce)));
                transform.localPosition = pos;
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
