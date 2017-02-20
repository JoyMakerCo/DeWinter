using UnityEngine;
using System.Collections;

public class AmbushedModal : MonoBehaviour {

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

        public void CreateAmbushedWorkTheRoomModal()
        {
            roomManager.WorkTheRoomModal(true);
        }
}