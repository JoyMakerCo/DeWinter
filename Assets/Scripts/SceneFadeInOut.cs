using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFadeInOut : MonoBehaviour {

    public Image sceneFadeImage;
    public float fadeSpeed;          // Speed that the screen fades to and from black.
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.

    // Use this for initialization
    void Start () {
        sceneFadeImage = this.GetComponent<Image>();
        sceneFadeImage.transform.localScale = new Vector3(1,1,1);
    }

    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
        {
            // ... call the StartScene function.
            StartScene();
        } else if (!sceneStarting)
        {
           sceneFadeImage.enabled = true;
           //Can't directly modify a Color's Alpha value... for some reason
           Color tempColor = Color.black;
           tempColor.a = 0.5f;
           sceneFadeImage.color = tempColor;
        } else
        {
           sceneFadeImage.color = Color.clear;
           sceneFadeImage.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SendMessage("CreateQuitGamePopUp");
        }
    }


    void FadeToClear()
    {
        // Lerp the colour of the texture between itself and transparent.
        sceneFadeImage.color = Color.Lerp(sceneFadeImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the texture between itself and black.
        sceneFadeImage.color = Color.Lerp(sceneFadeImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (sceneFadeImage.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the GUITexture.
            sceneFadeImage.color = Color.clear;
            sceneFadeImage.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }

    public void EndScene()
    {
        // Make sure the texture is enabled.
        sceneFadeImage.enabled = true;

        // Start fading towards black.
        FadeToBlack();
        // If the screen is almost black...
        if (sceneFadeImage.color.a >= 0.95f)
        {
            sceneFadeImage.color = Color.black;
            // The scene is no longer starting.
            
        }
                
    }
}
