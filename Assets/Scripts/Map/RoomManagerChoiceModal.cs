using UnityEngine;
using System.Collections;

namespace Ambition
{
	public class RoomManagerChoiceModal : MonoBehaviour
	{

	    public SceneFadeInOut sceneFader;
	    public RoomManager roomManager;

	    void Start()
	    {
	        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
	    }

	    public void Dismiss()
	    {
	        Destroy(transform.parent.gameObject);
	    }

	    public void CreateWorkTheRoomOrHostModal(bool isAmbush)
	    {
	        if (!roomManager.currentPlayerRoom.HostHere)
	        {
	            roomManager.WorkTheRoomModal(isAmbush);
	        } else
	        {
	            roomManager.WorkTheHostModal();
	        }
	    }

	    public void TryToMoveThrough()
	    {
	        if (!roomManager.currentPlayerRoom.HostHere)
	        {
	            roomManager.MoveThrough();
	        } else
	        {
	            Debug.Log("Can't move through the Host Room");
	        }
	    }
	}
}