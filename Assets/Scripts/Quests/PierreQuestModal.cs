using UnityEngine;
using System.Collections;

public class PierreQuestModal : MonoBehaviour {

    public PierreQuest quest;
    public PierreQuestInventoryList questList;

	public void AcceptQuest()
    {
        GameData.pierreQuestInventory.Add(quest);
        Debug.Log(quest.Name + " added to Quest Inventory!");
        questList.ClearInventoryButtons();
        questList.GenerateInventoryButtons();
    }
}