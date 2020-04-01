using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class FadeView : MonoBehaviour
    {
        public float FadeInSeconds = 1f;
        public float FadeOutSeconds = 1f;
        public OnStartAction OnStart = OnStartAction.Nothing;

        private CanvasGroup _canvasGroup;

        public enum OnStartAction
        {
            Nothing,
            FadeIn,
            FadeOut
        }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = true;
                _canvasGroup.ignoreParentGroups = false;
            }                 
            switch (OnStart)
            {
                case OnStartAction.FadeIn:
                    FadeIn();
                    break;
                case OnStartAction.FadeOut:
                    FadeOut();
                    break;
            }
        }

        public void FadeIn()
        {
            StopAllCoroutines();
            StartCoroutine(Fade(true, FadeInSeconds));
        }

        public void FadeIn(float time)
        {
            StopAllCoroutines();
            StartCoroutine(Fade(true, time));
        }

        public void FadeOut()
        {
            StopAllCoroutines();
            StartCoroutine(Fade(false, FadeOutSeconds));
        }

        public void FadeOut(float time)
        {
            StopAllCoroutines();
            StartCoroutine(Fade(false, time));
        }

        IEnumerator Fade(bool fadeIn, float time)
        {
            float delta = (fadeIn ? 1f : -1f) / time;
            float startAlpha = fadeIn ? 0f : 1f;
            for (float t = 0; t < time; t+=Time.deltaTime)
            {
                GetComponent<CanvasGroup>().alpha = startAlpha + (delta * t);
                yield return null;
            }
            GetComponent<CanvasGroup>().alpha = fadeIn ? 1f : 0f;
        }
    }
}
