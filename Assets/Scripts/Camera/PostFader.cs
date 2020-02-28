using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class PostFader : MonoBehaviour
    {
        public Shader Effect;
        public CanvasGroup Blocker; // Assigned from the main Canvas
        private const float DEFAULT_TIME = 1.0f;
        private Material _material;
        private float _fade = 1f;
        private bool _fadeIn = true;
        private bool _fading = false;

        // Creates a private material used to the effect
        void Awake ()
        {
            Blocker.interactable = true;
            Blocker.blocksRaycasts = true;
            Blocker.ignoreParentGroups = true;

            _material = new Material(Effect);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe<float>(GameMessages.FADE_IN, HandleFadeIn);
            AmbitionApp.Subscribe(GameMessages.INTERRUPT_FADE, HandleInterruptFade);
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
            if (_fading)
            {
                if (fadeIn == _fadeIn) return;
                else
                {
                    StopAllCoroutines();
                    Blocker.interactable = false;
                    AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
                }
            }
            _fadeIn = fadeIn;
            StartCoroutine(Fade(time));
        }

        private void HandleInterruptFade()
        {
            StopAllCoroutines();
            _fade = _fadeIn ? 1f : 0f;
            AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
        }

        // Postprocess the image
        IEnumerator Fade(float time)
       {
            if (_fadeIn)
            {
                for (float d = 1f / time; _fade < 1f; _fade += d * Time.deltaTime)
                {
                    yield return null;
                }
                _fade = 1f;
            }
            else
            {
                for (float d = 1f / time; _fade > 0f; _fade -= d * Time.deltaTime)
                {
                    yield return null;
                }
                _fade = 0f;
            }
            Blocker.interactable = _fadeIn;
            AmbitionApp.SendMessage(_fadeIn ? GameMessages.FADE_IN_COMPLETE : GameMessages.FADE_OUT_COMPLETE);
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            if (_fade >= 1f) Graphics.Blit (source, destination);
            else
            {
                _material.SetFloat("_fade", (float)_fade);
                Graphics.Blit (source, destination, _material);
            }
        }
    }
}
