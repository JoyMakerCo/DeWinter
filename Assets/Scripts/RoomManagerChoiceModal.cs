using UnityEngine;
using System.Collections;

public class RoomManagerChoiceModal : MonoBehaviour {

    public SceneFadeInOut sceneFader;
    public RoomManager roomManager;

    void Start()
    {
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    public void Dismiss()
    {
        Destroy(transform.parent.gameObject);
        GameData.activeModals--;
    }

    public void CreateWorkTheRoomOrHostModal(bool isAmbush)
    {
        if (!roomManager.currentPlayerRoom.hostHere)
        {
            roomManager.WorkTheRoomModal(isAmbush);
        } else
        {
            roomManager.WorkTheHostModal();
        }
    }

    public void TryToMoveThrough()
    {
        if (!roomManager.currentPlayerRoom.hostHere)
        {
            roomManager.MoveThrough();
        } else
        {
            Debug.Log("Can't move through the Host Room");
        }
    }
}
