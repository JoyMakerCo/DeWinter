using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour {

    public Image sceneFadeImage;
    public float fadeSpeed;          // Speed that the screen fades to and from black.

    // Use this for initialization
    void Start () {
        sceneFadeImage = this.GetComponent<Image>();
        sceneFadeImage.transform.localScale = Vector3.one;
		sceneFadeImage.color = Color.black;
		FadeToClear();
    }

    void Update()
    {
		sceneFadeImage.enabled = (sceneFadeImage.color != Color.clear || transform.childCount > 0);
    }


    void FadeToClear()
    {
		StartCoroutine(FadeTo(Color.clear));
	}

    void FadeToBlack()
    {
		StartCoroutine(FadeTo(Color.black));
    }


    public void EndScene()
    {
        // Start fading towards black.
        FadeToBlack();
    }

	IEnumerator FadeTo(Color c)
	{
		float t0 = Time.time;
		float t = 0;
		Color c0 = sceneFadeImage.color;
		while (t < fadeSpeed)
		{
			t = Time.time;
	        // Lerp the colour of the texture between itself and transparent.
			sceneFadeImage.color = Color.Lerp(c0, c,  t / fadeSpeed);
			yield return null;
	    }
		sceneFadeImage.color = c;
    }
}