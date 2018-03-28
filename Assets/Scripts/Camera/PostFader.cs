using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class PostFader : MonoBehaviour
    {
        private const float DEFAULT_TIME = 1.0f;
        public Shader Effect;
        private Material _material;
        private static float _fade = 1f;

        // Creates a private material used to the effect
        void Awake ()
        {
            _material = new Material(Effect);
            AmbitionApp.Subscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Subscribe(GameMessages.FADE_IN, HandleFadeIn);
        }

        void Start()
        {
            StartCoroutine(FadeIn(DEFAULT_TIME));
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe(GameMessages.FADE_OUT, HandleFadeOut);
            AmbitionApp.Unsubscribe(GameMessages.FADE_IN, HandleFadeIn);
        }
        
        private void HandleFadeOut()
        {
            StartCoroutine(FadeOut(DEFAULT_TIME));
        }
       
        private void HandleFadeIn()
        {
            StartCoroutine(FadeIn(DEFAULT_TIME));
        }
       
       IEnumerator FadeOut(float time)
       {
           if (_fade > 1f) _fade = 1f;
           for (float delta=_fade/time; _fade > 0f; _fade-=delta*Time.deltaTime)
           {
                yield return null;
           }
           _fade = 0f;
           AmbitionApp.SendMessage(GameMessages.FADE_OUT_COMPLETE);
       }

        // Postprocess the image
       IEnumerator FadeIn(float time)
       {
           if (_fade < 0f) _fade = 0f;
           for (float delta=(1f-_fade)/time; _fade < 1f; _fade+=delta*Time.deltaTime)
           {
                yield return null;
           }
           _fade = 1f;
           AmbitionApp.SendMessage(GameMessages.FADE_IN_COMPLETE);
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
