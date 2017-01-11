using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventOptionButton : MonoBehaviour {

    public int option;
    private Text myText;

    void Start()
    {
        myText = this.GetComponentInChildren<Text>();
    }

    void Update()
    {
        DisplayEventOption();
    }

    public void DisplayEventOption()
    {
        EventOption eventOption = GameData.selectedEvent.eventStages[GameData.selectedEvent.currentStage].stageEventOptions[option];
        if (eventOption.optionButtonText == null)         
        {
            myText.text = "Nothing";
            this.transform.localScale = new Vector3(0, 0, 0); //Shrink it until it doesn't exist
        } else if (eventOption.servantRequired != null)
        {
            if (GameData.servantDictionary[eventOption.servantRequired.Slot()].Hired())
            {
                myText.text = GameData.selectedEvent.eventStages[GameData.selectedEvent.currentStage].stageEventOptions[option].optionButtonText;
                this.transform.localScale = new Vector3(1, 1, 1); //Unshrink it to it's original size
            } else
            {
                myText.text = "Nothing";
                this.transform.localScale = new Vector3(0, 0, 0); //Shrink it until it doesn't exist
            }
        }
        else
        {
            myText.text = GameData.selectedEvent.eventStages[GameData.selectedEvent.currentStage].stageEventOptions[option].optionButtonText;
            this.transform.localScale = new Vector3(1, 1, 1); //Unshrink it to it's original size
        }
    }
}
