using UnityEngine;
using System.Collections;

public class PierreQuestInventoryList : MonoBehaviour {

    public int selectedQuest;
    public GameObject pierreQuestInventoryButtonPrefab;
    public SceneFadeInOut screenFader;

    // Use this for initialization
    void Start()
    {
        foreach (PierreQuest p in GameData.pierreQuestInventory)
        {
            p.daysLeft--;
            if (p.daysLeft <= 0)
            {
                GameData.pierreQuestInventory.Remove(p);
            }
        }
        GenerateInventoryButtons();
        selectedQuest = -1; // So nothing is selected at the start
        AssignNewQuestCheck();
    }

    public void GenerateInventoryButtons()
    {
        for (int i = 0; i < GameData.pierreQuestInventory.Count; i++)
        {
            GameObject button = GameObject.Instantiate(pierreQuestInventoryButtonPrefab);
            PierreQuestInventoryButton buttonStats = button.GetComponent<PierreQuestInventoryButton>();
            buttonStats.questID = i;
            button.transform.SetParent(this.transform, false);
            Debug.Log("Pierre Quest Button: " + i + " is made!");
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void AssignNewQuestCheck()
    {
        if (GameData.currentDay >= GameData.nextQuestDay)
        {
            //Actually Assigning the Quest
            if (GameData.pierreQuestInventory.Count < 3)
            {
                //Create the Quest
                PierreQuest newPierreQuest = new PierreQuest();
                //Send Modal for accepting or rejecting the new Quest
                object[] objectStorage = new object[2];
                objectStorage[0] = newPierreQuest;
                objectStorage[1] = this;
                screenFader.gameObject.SendMessage("CreateNewPierreQuestModal", objectStorage);
            }
            GameData.nextQuestDay += Random.Range(3, 6);
        }       
    }
}
