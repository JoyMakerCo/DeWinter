using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeWinter;

public class StartGameButtonController : MonoBehaviour
{
	// This performs all the actions necessary for starting the game;
	public void StartGame()
	{
		EventModel emod = DeWinterApp.GetModel<EventModel>();
		emod.SelectedEvent = emod.eventInventories["intro"][0];
		DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
	}
}