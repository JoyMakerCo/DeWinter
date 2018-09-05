using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {

    [System.Serializable] //Serializes the Class for Inspector Use
	public class AnimationSettings
    {
        public enum OpenStyle { WidthToHeight, HeightToWidth, HeightAndWidth};
        public OpenStyle openStyle; // How the Tooltip opens (The Animation Style)
        public float widthSmooth = 4.6f, heightSmooth = 4.6f; // How fast the text box will open in various directions
        public float textSmooth = 0.1f; // How fast the text appears

        [HideInInspector]
        public bool widthOpen = false, heightOpen = false, textOpen = false; //May be opening only one at a time

        public void Initialize() // An initialize is called each time the animation starts. Needs to reset to these every time the Animation is called.
        {
            widthOpen = false;
            heightOpen = false;
            textOpen = false;
        }
    }

    [System.Serializable]
    public class UISettings
    {
        public Image textBox; // The Image/Box Background
        public Text text; // The Tooltip Message
        public Vector2 initialBoxSize = new Vector2(0.25f, 0.25f);
        public Vector2 openedBoxSize = new Vector2(400, 200);
        public float snapToSizeDistance = 0.25f; // This has to do with Lerps animation for the box and just finishing it off
        public float snapToTextFadeDistance = 0.25f;// This has to do with the Lerps fade in for the text and just finishing it off
        public float lifeSpan = 5; // How long the Tooltip sticks around

        [HideInInspector]
        public bool opening = false;
        [HideInInspector]
        public Color textColor;
        [HideInInspector]
        public Color textBoxColor;
        [HideInInspector]
        public RectTransform textBoxRect;
        [HideInInspector]
        public Vector2 currentSize;

        public void Initialize() // An initialize is called each time the animation starts. Needs to reset to these every time the Animation is called.
        {
            textBoxRect = textBox.GetComponent<RectTransform>();
            textBoxRect.sizeDelta = initialBoxSize;
            currentSize = textBoxRect.sizeDelta;
            opening = false;
            // Set the Text Color Alpha back to 0
            textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
            textBoxColor = textBox.color;
            textBoxColor.a = 1;
            textBox.color = textBoxColor;

            textBox.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }
    public AnimationSettings animSettings = new AnimationSettings();
    public UISettings uiSettings = new UISettings();

    float lifeTimer = 0;

    void Start()
    {
        animSettings.Initialize();
        uiSettings.Initialize();
    }
   
    // This method is called when the Button is clicked

    public void StartOpen()
    {
        uiSettings.opening = true;
        uiSettings.textBox.gameObject.SetActive(true);
        uiSettings.text.gameObject.SetActive(true);
    }

    void Update()
    {
        if (uiSettings.opening)
        {
            OpenToolTip();
            if (animSettings.widthOpen && animSettings.heightOpen)
            {
                lifeTimer += Time.deltaTime;
                if (lifeTimer > uiSettings.lifeSpan)
                {
                    FadeToolTipOut();
                }
                else
                {
                    FadeTextIn();
                }
            }
        }
    }

    void OpenToolTip()
    {
        //Checks which Opening Style to use
        switch(animSettings.openStyle)
        {
            case AnimationSettings.OpenStyle.HeightToWidth:
                OpenHeightToWidth();
                break;
            case AnimationSettings.OpenStyle.WidthToHeight:
                OpenWidthToHeight();
                break;
            case AnimationSettings.OpenStyle.HeightAndWidth:
                OpenHeightAndWidth();
                break;
            default:
                Debug.LogError("No animation is set for the selected open style!");
                break;
            
        }
        uiSettings.textBoxRect.sizeDelta = uiSettings.currentSize;
    }

    void OpenWidthToHeight()
    {
        if (!animSettings.widthOpen)
        {
            OpenWidth();
        } else
        {
            OpenHeight();
        }
    }

    void OpenHeightToWidth()
    {
        if (!animSettings.heightOpen)
        {
            OpenHeight();
        } else
        {
            OpenWidth();
        }
    }

    void OpenHeightAndWidth()
    {
        if (!animSettings.widthOpen)
        {
            OpenWidth();
        }
        if (!animSettings.heightOpen)
        {
            OpenHeight();
        }
    }

    void OpenWidth()
    {
        uiSettings.currentSize.x = Mathf.Lerp(uiSettings.currentSize.x, uiSettings.openedBoxSize.x, animSettings.widthSmooth * Time.deltaTime);

        //If the Current Width is close enough to the Target Width then just snap to the Target and complete the animation.
        if (Mathf.Abs(uiSettings.currentSize.x - uiSettings.openedBoxSize.x) < uiSettings.snapToSizeDistance)
        {
            uiSettings.currentSize.x = uiSettings.openedBoxSize.x;
            animSettings.widthOpen = true;
        }
    }

    void OpenHeight()
    {
        uiSettings.currentSize.y = Mathf.Lerp(uiSettings.currentSize.y, uiSettings.openedBoxSize.y, animSettings.heightSmooth * Time.deltaTime);

        //If the Current Width is close enough to the Target Width then just snap to the Target and complete the animation.
        if (Mathf.Abs(uiSettings.currentSize.y - uiSettings.openedBoxSize.y) < uiSettings.snapToSizeDistance)
        {
            uiSettings.currentSize.y = uiSettings.openedBoxSize.y;
            animSettings.heightOpen = true;
        }
    }

    void FadeTextIn()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a, 1, animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
    }

    public void FadeToolTipOut()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
        uiSettings.textBoxColor.a = Mathf.Lerp(uiSettings.textBoxColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.textBox.color = uiSettings.textBoxColor;

        if (uiSettings.textBoxColor.a < 0.01f)
        {
            uiSettings.opening = false;
            animSettings.Initialize();
            uiSettings.Initialize();
            lifeTimer = 0;
        }
    }
}
