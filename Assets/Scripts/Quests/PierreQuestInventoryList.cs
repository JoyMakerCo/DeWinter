using UnityEngine;
using System;
using System.Collections;
using Ambition;

public class PierreQuestInventoryList : MonoBehaviour {
    public PierreQuest quest;
    public GameObject pierreQuestInventoryButtonPrefab;
    private QuestModel _model;

    // Use this for initialization
    void Start()
    {
		_model = AmbitionApp.GetModel<QuestModel>();
        //_model.Quests.ForEach(q => q.daysLeft--);
        //_model.Quests.RemoveAll(q => q.daysLeft <= 0); // hmm
        GenerateInventoryButtons();
		//AmbitionApp.Subscribe<DateTime>(HandleDay);
    }

    public void GenerateInventoryButtons()
    {
        GameObject button;
        //foreach (PierreQuest quest in _model.Quests)
        //{
            button = GameObject.Instantiate(pierreQuestInventoryButtonPrefab, this.transform);
            button.GetComponent<PierreQuestInventoryButton>().quest = _model.CurrentQuest;
        //}
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

//	private void HandleDay(DateTime day)
//	{
//		if (day >= _model.NextQuestDay)
//        {
//            //Actually Assigning the Quest
//			if (GameData.pierreQuestInventory.Count < 3 && AmbitionApp.GetModel<GameModel>().Level > 0)
//            {
//                //Create the Quest
//                PierreQuest newPierreQuest = new PierreQuest();
//                //Send Modal for accepting or rejecting the new Quest
//                object[] objectStorage = new object[2];
//                objectStorage[0] = newPierreQuest;
//                objectStorage[1] = this;
//// TODO
    //            // screenFader.gameObject.SendMessage("CreateNewPierreQuestModal", objectStorage);
    //        }
    //        _model.NextQuestDay = day.AddDays( Util.RNG.Generate(3, 6));
    //    }       
    //}
}
