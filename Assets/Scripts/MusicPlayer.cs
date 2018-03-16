using UnityEngine;
using System.Collections;

namespace Ambition
{
	public class MusicPlayer : MonoBehaviour
	{
		private const float DEFAULT_FADE = 1.0f;
		private static GameObject _instance = null;
		private AudioSource _src;
		private AudioSource _swapSrc;
		private AmbientClip _clip;

		void Awake()
		{
			if (_instance != null) Destroy(this.gameObject);
			else {
				AudioSource[] sources = gameObject.GetComponents<AudioSource>();
				_src = sources[0];
				_swapSrc = sources[1];
				_instance = this.gameObject;
				GameObject.DontDestroyOnLoad(_instance);
				AmbitionApp.Subscribe<AmbientClip>(AudioMessages.PLAY_MUSIC, HandlePlay);
				AmbitionApp.Subscribe<float>(AudioMessages.STOP_MUSIC, HandleStop);
				AmbitionApp.Subscribe(AudioMessages.STOP_MUSIC, HandleStopNow);
			}
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<AmbientClip>(AudioMessages.PLAY_MUSIC, HandlePlay);
			AmbitionApp.Unsubscribe<float>(AudioMessages.STOP_MUSIC, HandleStop);
			AmbitionApp.Unsubscribe(AudioMessages.STOP_MUSIC, HandleStopNow);
		}

		private void HandlePlay(AmbientClip clip)
		{
			if (clip != null && (!_src.isPlaying || _clip != clip))
			{
				float delay = _src.isPlaying ? DEFAULT_FADE : 0;
				if (_src.isPlaying)
				{
					AudioSource fadeout = _src;
					_src = this.gameObject.AddComponent<AudioSource>();
					StartCoroutine(FadeOut(fadeout, delay));
				}
				_clip = clip;
				if (_clip.Intro != null)
				{
					_src.clip = clip.Intro;
					_src.loop = false;
					_src.PlayDelayed(delay);
					_swapSrc.clip = clip.Loop;
					_swapSrc.PlayDelayed(delay + _clip.Intro.length);
					_swapSrc.loop = true;
					StartCoroutine(WaitToSwap());
				}
				else
				{
					_src.clip = _clip.Loop;
					_src.loop = true;
					_src.PlayDelayed(delay);
				}
			}
		}

		private void HandleStop(float fadeout)
		{
			if (fadeout <= 0) HandleStopNow();
			else
			{
				StopCoroutine(WaitToSwap());
				StartCoroutine(FadeOut(_src, fadeout));
			}
		}

		private void HandleStopNow()
		{
			StopAllCoroutines();
			_src.Stop();
		}

		IEnumerator FadeOut(AudioSource source, float time=DEFAULT_FADE)
		{
			float multiplier = source.volume/time;
			while (source.volume > 0) {
				source.volume -= multiplier * Time.deltaTime;
				yield return null;
			}
			source.Stop();
			if (source != _src && source != _swapSrc) Destroy(source);
		}

        IEnumerator WaitToSwap()
        {
			while(_src.isPlaying)
				yield return null;
			AudioSource tmp = _src;
			_src = _swapSrc;
			_swapSrc = tmp;
        }
	}
}
