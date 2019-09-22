using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PierreQuestInventoryButton : MonoBehaviour
{
    public PierreQuest quest;
    private Text myDescriptionText;
    private Text myTimeRemainingText;
    private Outline myOutline; // This is for highlighting buttons

    PierreQuestInventoryList pierreQuestInventoryList;

    void Start()
    {
        myDescriptionText = this.transform.Find("DescriptionText").GetComponent<Text>();
        myTimeRemainingText = this.transform.Find("DaysLeftText").GetComponent<Text>();
        myOutline = this.GetComponent<Outline>();
        pierreQuestInventoryList = this.transform.parent.GetComponent<PierreQuestInventoryList>();
    }

    private void SelectQuest(PierreQuest quest)
    {
        myDescriptionText.text = quest?.Name;
        myTimeRemainingText.text = (quest?.daysLeft ?? '0') + "/" + (quest?.daysTimeLimit ?? '0');
        myOutline.effectColor = (pierreQuestInventoryList.quest == quest)
            ? Color.yellow
            : Color.clear;
    }

    public void SetInventoryItem() => pierreQuestInventoryList.quest = quest;
}
