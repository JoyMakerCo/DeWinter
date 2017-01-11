using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StyleTracker : MonoBehaviour {
    public GameObject styleChangeModal;
    private Text myText;
    public GameObject screenFader;

    void Start()
    {
        myText = this.GetComponent<Text>();
        //This is just to force an update at the beginning.
        myText.text = GameData.currentStyle;
        //Is it Style Switch Time?
        if (GameData.currentDay >= GameData.nextStyleSwitch)
        {
            //Pop Up Window
            //Can only pass an object, so make it an Array! Hax?            
            object[] stringStorage = new object[2];
            stringStorage[0] = GameData.currentStyle;
            stringStorage[1] = GameData.nextStyle;
            screenFader.gameObject.SendMessage("CreateFashionPopUp", stringStorage);
            //Actually switching styles
            GameData.currentStyle = GameData.nextStyle;
            GameData.nextStyle = SwitchStyle(GameData.currentStyle); //First version is initially set in Game Data. This is super hax. Fix it.
            GameData.lastStyleSwitch = GameData.nextStyleSwitch;
            GameData.nextStyleSwitch = GameData.lastStyleSwitch + Random.Range(6, 9);
        }
    }

    void Update()
    {
        myText.text = GameData.currentStyle;
    }

    string SwitchStyle(string lastStyle)
    {
        string selectedStyle = lastStyle;
        while (selectedStyle == lastStyle)
        {
            int styleNumber = Random.Range(1, 4); //Fucking exclusive Random Ints
            switch (styleNumber)
            {
                case 1:
                    selectedStyle = "Frankish";
                    break;
                case 2:
                    selectedStyle = "Venezian";
                    break;
                default:
                    selectedStyle = "Catalan";
                    break;
            }
        }  
        return selectedStyle;
    }
}
