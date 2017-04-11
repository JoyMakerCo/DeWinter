using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeWinter;

public class WorkTheRoomPopUpModalController : MonoBehaviour {

    public EncounterViewMediator workTheRoomManager;

	//Is used when the Pop Up is dismissed, restarting the the Conversation in the game
	public void ResumeConversation () {
        workTheRoomManager.StartCoroutine(workTheRoomManager.ConversationStartTimerWait());
    }
}
