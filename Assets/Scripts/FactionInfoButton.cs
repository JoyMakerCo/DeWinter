using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FactionInfoButton : MonoBehaviour {

    private Text text;
    private Image image;

    public FactionInfoTextController textController;
    public string faction;
    public enum InfoType {Allegiance, Power};
    public InfoType infoType;

    // Use this for initialization
    void Start () {
        text = this.transform.Find("Text").GetComponent<Text>();
        image = this.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        DisplayButton();
	}

    void DisplayButton()
    {
        if(infoType == InfoType.Allegiance)
        {
            if (GameData.factionList[faction].PlayerReputationLevel() >= 7)
            {
                text.text = "";
                image.color = Color.clear;
            }
            else
            {
                if (textController.availableSpymasterTestTheWaters)
                {
                    text.text = "Learn Current Allegiance (Spymaster - Free!)";
                    image.color = Color.white;
                } else if (textController.availableTestTheWaters && GameData.moneyCount >= textController.testTheWatersCost)
                {
                    text.text = "Learn Current Allegiance (£" + textController.testTheWatersCost + ")";
                    image.color = Color.white;
                } else
                {
                    text.text = "Learn Current Allegiance (£" + textController.testTheWatersCost + ")";
                    image.color = Color.gray;
                }
            }
        } else
        {
            if (GameData.factionList[faction].PlayerReputationLevel() >= 5)
            {
                text.text = "";
                image.color = Color.clear;
            }
            else
            {
                if (textController.availableSpymasterTestTheWaters)
                {
                    text.text = "Learn Current Power (Spymaster - Free!)";
                    image.color = Color.white;
                } else if (textController.availableTestTheWaters && GameData.moneyCount >= textController.testTheWatersCost)
                {
                    text.text = "Learn Current Power (£" + textController.testTheWatersCost + ")";
                    image.color = Color.white;
                } else
                {
                    text.text = "Learn Current Power (£" + textController.testTheWatersCost + ")";
                    image.color = Color.gray;
                }
            }
        }
    }
}
