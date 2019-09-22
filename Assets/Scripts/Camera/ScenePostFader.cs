using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Ambition
{
    public class ScenePostFader : MonoBehaviour
    {
        public float FadeOutTime = 1.0f;
        public float FadeInTime = 1.0f;

        public string[] TriggerFadeIn;
        public string[] TriggerFadeOut;

        public string[] OnFadeOut;
        public string[] OnFadeIn;

        public Shader Effect;

        private Material _material;
        private static float _fade = 1f;

        // Creates a private material used to the effect
        void Awake ()
        {
            _material = new Material(Effect);
            foreach (string trigger in TriggerFadeIn)
            {
                AmbitionApp.Subscribe(trigger, HandleFadeIn);
            }
            foreach (string trigger in TriggerFadeOut)
            {
                AmbitionApp.Subscribe(trigger, HandleFadeOut);
            }
        }

        void OnDestroy()
        {
            foreach (string trigger in TriggerFadeIn)
            {
                AmbitionApp.Unsubscribe(trigger, HandleFadeIn);
            }
            foreach (string trigger in TriggerFadeOut)
            {
                AmbitionApp.Unsubscribe(trigger, HandleFadeOut);
            }
        }

        private void HandleFadeOut()
        {
            if (_fade <= 0f) AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);
            else
            {
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }
        }
       
        private void HandleFadeIn()
        {
            if (_fade >= 1f) AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
            else
            {
                StopAllCoroutines();
                StartCoroutine(FadeIn());
            }
        }

       IEnumerator FadeOut()
       {
           for (float delta=_fade/FadeOutTime; _fade > 0f; _fade-=delta*Time.deltaTime)
           {
                yield return null;
           }
           _fade = 0f;
            Array.ForEach(OnFadeOut, AmbitionApp.SendMessage);
       }

        // Postprocess the image
       IEnumerator FadeIn()
       {
            for (float delta = (1f - _fade) / FadeInTime; _fade < 1f; _fade += delta * Time.deltaTime)
            {
                yield return null;
            }
           _fade = 1f;
            Array.ForEach(OnFadeIn, AmbitionApp.SendMessage);
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
