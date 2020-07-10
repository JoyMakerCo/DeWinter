using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class PostFader : MonoBehaviour
    {
        private const string FADE_VAR = "_fade";
        private const float DEFAULT_TIME = 1.0f;
        public Shader Effect;
        private Material _material;
        private float _fade = 0f;
        private bool _fadeIn;

        // Creates a private material used to the effect
        void Awake ()
        {
            _material = new Material(Effect);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe(GameMessages.INTERRUPT_FADE, HandleInterruptFade);
        }

        private void OnEnable()
        {
            HandleFadeIn();
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
            StopAllCoroutines();
            if (_fadeIn != fadeIn && _fade > 0f && _fade < 1f)
            {
                AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
            }
            _fadeIn = fadeIn;
            if (_fadeIn) StartCoroutine(FadeIn(time * (1f - _fade)));
            else StartCoroutine(FadeOut(time * _fade));
        }

        // Postprocess the image
       IEnumerator FadeOut(float time)
       {
            for (float d = _fade / time; _fade > 0f; _fade -= d*Time.deltaTime)
            {
                yield return null;
            }
            _fade = 0f;
           AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);
        }

        // Postprocess the image
        IEnumerator FadeIn(float time)
        {
            for (float d = (1f-_fade) / time; _fade < 1f; _fade += d * Time.deltaTime)
            {
                yield return null;
            }
            _fade = 1f;
            AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
        }

        private void HandleInterruptFade()
        {
            StopAllCoroutines();
            _fade = _fadeIn ? 1f : 0f;
            AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            if (_fade >= 1f) Graphics.Blit (source, destination);
            else
            {
                _material.SetFloat(FADE_VAR, _fade);
                Graphics.Blit (source, destination, _material);
            }
        }
    }
}
