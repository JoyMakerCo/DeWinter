using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeWinter;

public class StartGameButtonController : MonoBehaviour
{
    public LevelManager levelManager; //Filled in by the PopUpManager
    public DismissPopUp dismiss;
	
	// This performs all the actions necessary for starting the game;
	void StartGame()
	{
		OutfitInventory.StockInventory();
        dismiss.Dismiss();
        levelManager.LoadLevel("Game_Estate");
        DeWinterApp.SendMessage(CalendarConsts.ADVANCE_DAY);
	}
}