using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeWinter;

public class StartGameButtonController : MonoBehaviour
{
	// This performs all the actions necessary for starting the game;
	public void StartGame()
	{
		DeWinterApp.SendMessage<string>(GameMessages.LOAD_SCENE, "Game_Estate");
        DeWinterApp.SendMessage(CalendarConsts.ADVANCE_DAY);
		OutfitInventory.StockInventory();
	}
}