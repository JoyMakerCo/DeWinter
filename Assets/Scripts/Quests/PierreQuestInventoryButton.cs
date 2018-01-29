using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PierreQuestInventoryButton : MonoBehaviour {
    public int questID;
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

    void Update()
    {
        DisplayQuestStats(questID);
        if (pierreQuestInventoryList.selectedQuest == questID)
        {
            myOutline.effectColor = Color.yellow;
        }
        else
        {
            myOutline.effectColor = Color.clear;
        }
    }

    public void DisplayQuestStats(int qID)
    {
        if (GameData.pierreQuestInventory.ElementAtOrDefault(qID) != null)
        {
            myDescriptionText.text = GameData.pierreQuestInventory[qID].Name;
            myTimeRemainingText.text = GameData.pierreQuestInventory[qID].daysLeft + "/" + GameData.pierreQuestInventory[qID].daysTimeLimit;
        }
    }

    public void SetInventoryItem()
    {
        Debug.Log("Selected Pierre Quest: " + questID.ToString());
        pierreQuestInventoryList.selectedQuest = questID;
    }
}
