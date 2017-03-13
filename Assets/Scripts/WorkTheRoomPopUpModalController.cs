using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkTheRoomPopUpModalController : MonoBehaviour {

    public WorkTheRoomManager workTheRoomManager;

	//Is used when the Pop Up is dismissed, restarting the the Conversation in the game
	public void ResumeConversation () {
        workTheRoomManager.StartCoroutine(workTheRoomManager.ConversationStartTimerWait());
    }
}
