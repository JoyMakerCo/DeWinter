using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FactionInfoButton : MonoBehaviour {

    private Text text;
    private Image image;

    public FactionInfoTextController textController;
    public enum InfoType {Allegiance, Power};
    public InfoType infoType;

    // Use this for initialization
    void Start () {
        text = this.transform.Find("Text").GetComponent<Text>();
        image = this.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (textController.availableSpymasterTestTheWaters)
        {
            if (infoType == InfoType.Allegiance)
            {
                text.text = "Learn Current Allegiance (Spymaster - Free!)";
            }
            else
            {
                text.text = "Learn Current Power (Spymaster - Free!)";
            }
            image.color = Color.white;
        } else if (textController.availableTestTheWaters && GameData.moneyCount >= textController.testTheWatersCost)
        {
            if (infoType == InfoType.Allegiance)
            {
                text.text = "Learn Current Allegiance (£" + textController.testTheWatersCost + ")";
            }
            else
            {
                text.text = "Learn Current Power (£" + textController.testTheWatersCost + ")";
            }
            image.color = Color.white;
        } else
        {
            if (infoType == InfoType.Allegiance)
            {
                text.text = "Learn Current Allegiance (£" + textController.testTheWatersCost + ")";
            } else
            {
                text.text = "Learn Current Power (£" + textController.testTheWatersCost + ")";
            }
            image.color = Color.gray;
        }
	}
}
