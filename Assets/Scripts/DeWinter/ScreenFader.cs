using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace DeWinter
{
	public class ScreenFader : MonoBehaviour
	{
		public Image FaderImage;
		private string _sceneId=null;
		private IEnumerator _activeCoroutine=null;

		void Start ()
		{
			DeWinterApp.Subscribe<string>(GameMessages.CHANGE_SCENE, HandleScene);
			DeWinterApp.Subscribe<float>(GameMessages.FADE_IN, FadeIn);
			DeWinterApp.Subscribe<float>(GameMessages.FADE_OUT, FadeOut);
		}

		void Destroy()
		{
			DeWinterApp.Unsubscribe<string>(GameMessages.CHANGE_SCENE, HandleScene);
			DeWinterApp.Unsubscribe<float>(GameMessages.FADE_IN, FadeIn);
			DeWinterApp.Unsubscribe<float>(GameMessages.FADE_OUT, FadeOut);
		}

		private void HandleScene(string sceneId)
		{
			FadeOut(GameConsts.SCENE_CHANGE_SECONDS);
			_sceneId = sceneId;
		}

		private void FadeIn(float seconds)
		{
			StopCoroutine(_activeCoroutine);
			StartCoroutine(_activeCoroutine = Fade(false, seconds));
		}

		private void FadeOut(float seconds)
		{
			StopCoroutine(_activeCoroutine);
			StartCoroutine(_activeCoroutine = Fade(true, seconds));
		}

		IEnumerator Fade(bool isFadeOut, float seconds)
		{
			float startTime = Time.time;
			Color startColor = isFadeOut ? Color.clear : Color.black;
			Color endColor = isFadeOut ? Color.black : Color.clear;
			for (float t=0.0f; t<seconds; t+=Time.time-startTime)
			{
				FaderImage.color = Color.Lerp(startColor, endColor, t/seconds);
				yield return null;
			}
			FaderImage.color = endColor;
			_activeCoroutine = null;
			DeWinterApp.SendMessage(isFadeOut ? GameMessages.FADE_OUT_COMPLETE : GameMessages.FADE_IN_COMPLETE);
		}
	}
}