using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    public class PostFader : MonoBehaviour
    {
        public RawImage FaderImage;
        public CanvasGroup canvasGroup;
        private const float DEFAULT_TIME = 1.0f;
        private bool _fadeIn;
        private float _blockerAlpha;

        void Start ()
        {
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe(GameMessages.INTERRUPT_FADE, HandleInterruptFade);
            _blockerAlpha = FaderImage.color.a;
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Unsubscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Unsubscribe<float>(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Unsubscribe(GameMessages.INTERRUPT_FADE, HandleInterruptFade);
        }

        private void HandleFadeOut() => HandleFade(false, DEFAULT_TIME);
        private void HandleFadeIn() => HandleFade(true, DEFAULT_TIME);
        private void HandleFadeOut(float time) => HandleFade(false, time);
        private void HandleFadeIn(float time) => HandleFade(true, time);

        private void HandleFade(bool fadeIn, float time)
        {
            if (_fadeIn != fadeIn)
            {
                StopAllCoroutines();
                if (_fadeIn && _blockerAlpha > 0f)
                {
                    _blockerAlpha = FaderImage.color.a;
                    AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
                }
                else if (!_fadeIn && _blockerAlpha < 1f)
                {
                    FaderImage.color = Color.black;
                    _blockerAlpha = 1f;
                    AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);
                }
                _fadeIn = fadeIn;
                if (_fadeIn) StartCoroutine(FadeIn(time * _blockerAlpha));
                else StartCoroutine(FadeOut(time * (1f - _blockerAlpha)));
            }
        }

       IEnumerator FadeIn(float time)
       {
            Color color = Color.black;
            for (float d = _blockerAlpha / time; _blockerAlpha > 0f; _blockerAlpha -= d*Time.deltaTime)
            {
                canvasGroup.blocksRaycasts = false;
                color.a = _blockerAlpha;
                FaderImage.color = color;
                yield return null;
            }
            FaderImage.color = Color.clear;
            _blockerAlpha = 0f;
            canvasGroup.blocksRaycasts = true;
            AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
        }

        IEnumerator FadeOut(float time)
        {
            Color color = Color.black;
            for (float d = (1f-_blockerAlpha) / time; _blockerAlpha < 1f; _blockerAlpha += d * Time.deltaTime)
            {
                canvasGroup.blocksRaycasts = false;
                color.a = _blockerAlpha;
                FaderImage.color = color;
                yield return null;
            }
            _blockerAlpha = 1f;
            FaderImage.color = Color.black;
            AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);
        }

        private void HandleInterruptFade()
        {
            Color color = Color.black;
            StopAllCoroutines();
            color.a = _blockerAlpha = _fadeIn ? 0f : 1f;
            canvasGroup.blocksRaycasts = !_fadeIn;
            FaderImage.color = color;
            AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
        }
    }
}
