using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

// TODO: Style switching handled by command
public class StyleTracker : MonoBehaviour {
    public GameObject styleChangeModal;
    private Text myText;
    public GameObject screenFader;

    void Start()
    {
    	CalendarModel model = DeWinterApp.GetModel<CalendarModel>();
		InventoryModel invModel = DeWinterApp.GetModel<InventoryModel>();

        myText = this.GetComponent<Text>();
        //This is just to force an update at the beginning.
        myText.text = GameData.currentStyle;
        //Is it Style Switch Time?
        if (model.Day >= model.NextStyleSwitchDay)
        {
            //Pop Up Window
            //Can only pass an object, so make it an Array! Hax?            
            object[] stringStorage = new object[2];
            stringStorage[0] = GameData.currentStyle;
			stringStorage[1] = invModel.NextStyle;
            screenFader.gameObject.SendMessage("CreateFashionPopUp", stringStorage);
            //Actually switching styles
			invModel.CurrentStyle = invModel.NextStyle;
			invModel.NextStyle = SwitchStyle(GameData.currentStyle); //First version is initially set in Game Data. This is super hax. Fix it.
			model.NextStyleSwitchDay = model.Day + Random.Range(6, 9);
        }
    }

    // TODO: Respond to message
    void Update()
    {
        myText.text = GameData.currentStyle;
    }

    string SwitchStyle(string lastStyle)
    {
        string selectedStyle;
		switch (Random.Range(0,2))
        {
            case 0:
                selectedStyle = "Frankish";
                break;
            default:
                selectedStyle = "Venezian";
                break;
        }
		return selectedStyle == lastStyle ? "Catalan" : selectedStyle;
    }
}
