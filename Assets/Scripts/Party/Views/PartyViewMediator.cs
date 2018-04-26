using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Ambition
{
	public class PartyViewMediator : MonoBehaviour
	{
		public GameObject MapView;
		public GameObject RoomView;

		// Dependency Injection
		private MessageSvc _msg = App.Service<MessageSvc>();
		private UFlow.UFlowSvc _uflow = App.Service<UFlow.UFlowSvc>();
		private GameObject _roomView;

		// Use this for initialization
		void Awake()
		{
			_roomView = GameObject.Instantiate(RoomView, this.transform);
			_msg.Subscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			_msg.Subscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_ROOM, GoToRoom);
			AmbitionApp.Unsubscribe(PartyMessages.SHOW_MAP, GoToMap);
		}

		void Start ()
		{
			AmbitionApp.SendMessage(PartyMessages.SHOW_MAP);
			AmbitionApp.SendMessage(PartyMessages.START_PARTY);
		}

		private void GoToRoom()
		{
			MapView.SetActive(false);
			_roomView.SetActive(true);
			_uflow.InvokeMachine("ConversationController");
		}

		private void GoToMap()
		{
			MapView.SetActive(true);
			_roomView.SetActive(false);
		}
	}
}
