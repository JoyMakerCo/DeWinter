using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NextStyleTracker : MonoBehaviour {

    private Text myText;

    void Start()
    {
        myText = this.GetComponent<Text>();
        //This is just to force an update at the beginning.
        updateStyle();
    }

    void Update () {
        updateStyle();
    }

    public void updateStyle()
    {
        if (GameData.servantDictionary["Seamstress"].Hired())
        {
            myText.text = GameData.nextStyle;
        }
        else
        {
            myText.text = "Unknown";
        }
        
    }
}
