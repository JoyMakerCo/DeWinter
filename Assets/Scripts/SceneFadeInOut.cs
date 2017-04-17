using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class SceneFadeInOut : MonoBehaviour
	{
	    public float fadeSpeed; // Fade duration in seconds
		private Image _sceneFadeImage;

	    // Use this for initialization
	    void Awake () {
	        _sceneFadeImage = this.GetComponent<Image>();
	        _sceneFadeImage.transform.localScale = Vector3.one;
	        DeWinterApp.Subscribe(GameMessages.FADE_IN, FadeToClear);
			DeWinterApp.Subscribe(GameMessages.FADE_OUT, FadeToBlack);
	    }

	    void Start()
	    {
			FadeToClear();
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe(GameMessages.FADE_IN, FadeToClear);
			DeWinterApp.Unsubscribe(GameMessages.FADE_OUT, FadeToBlack);
	    }

	    void FadeToClear()
	    {
	    	StopAllCoroutines();
			StartCoroutine(Fade(false));
		}

	    void FadeToBlack()
	    {
			StopAllCoroutines();
			StartCoroutine(Fade(true));
	    }


	    public void EndScene()
	    {
	        // Start fading towards black.
	        FadeToBlack();
	    }

		IEnumerator Fade(bool ToBlack)
		{
			float t0 = Time.time;
			float t = 0;
			Color c0 = ToBlack ? Color.clear : Color.black;
			Color c1 = ToBlack ? Color.black : Color.clear;
			_sceneFadeImage.enabled = true;
			while (t < fadeSpeed)
			{
				t = Time.time;
		        // Lerp the colour of the texture between itself and transparent.
				_sceneFadeImage.color = Color.Lerp(c0, c1,  t / fadeSpeed);
				yield return null;
		    }
			DeWinterApp.SendMessage(ToBlack ? GameMessages.FADE_OUT_COMPLETE : GameMessages.FADE_IN_COMPLETE);
			_sceneFadeImage.enabled = ToBlack;
			_sceneFadeImage.color = c1;
	    }
	}
}