using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ambition;

public class FactionInfoButton : MonoBehaviour {

    private Text text;
    private Image image;

    public FactionInfoTextController textController;
    public FactionType faction;
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
    	FactionModel model = AmbitionApp.GetModel<FactionModel>();
        GameModel game = AmbitionApp.GetModel<GameModel>();
        if (infoType == InfoType.Allegiance)
        {
            if (model[faction].Level >= 8)
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
                } else if (textController.availableTestTheWaters && game.Livre >= textController.testTheWatersCost)
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
            if (model[faction].Level >= 6)
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
                } else if (textController.availableTestTheWaters && game.Livre >= textController.testTheWatersCost)
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
