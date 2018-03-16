using System.Collections;
using UnityEngine;

namespace Ambition
{
    public class AmbientAudioPlayer : MonoBehaviour
    {
        public bool StartOnEnable=true;
        public AmbientClip[] Clips;

        private AudioSource _src;

        void Awake()
        {
            _src = GetComponent<AudioSource>();
        }

        void OnEnable()
        {
            if (StartOnEnable) Play();
        }

        void OnDisable() { Stop(); }

        public void Play()
        {
            AmbientClip config = Clips[Util.RNG.Generate(Clips.Length)];
            StartCoroutine(PlayConfig(config));
        }

        public void Stop()
        {
            StopAllCoroutines();
            _src.Stop();
        }

        IEnumerator PlayConfig(AmbientClip config)
        {
            _src.clip = config.Intro;
            _src.loop = false;
            _src.Play();
            yield return new WaitForSeconds(_src.clip.length);
            _src.clip = config.Loop;
            _src.loop = true;
            _src.Play();
            yield return null;
        }
    }
}
