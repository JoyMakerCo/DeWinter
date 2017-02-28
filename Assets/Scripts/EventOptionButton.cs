using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class EventOptionButton : MonoBehaviour {

    public int option;
    private Text myText;
    private EventModel _eventModel;

    void Start()
    {
		_eventModel = DeWinterApp.GetModel<EventModel>();
        myText = this.GetComponentInChildren<Text>();
    }

    void Update()
    {
        DisplayEventOption();
    }

    public void DisplayEventOption()
    {
		Event selectedEvent = _eventModel.SelectedEvent;
		EventOption eventOption = selectedEvent.eventStages[selectedEvent.currentStage].stageEventOptions[option];
		bool show = (eventOption.optionButtonText != null);


		if (show && eventOption.servantRequired != null)
		{
			ServantModel smod = DeWinterApp.GetModel<ServantModel>();
			show = smod.Hired.ContainsKey(eventOption.servantRequired);
		}
		if (show) myText.text = eventOption.optionButtonText;
		this.gameObject.SetActive(show);
    }
}