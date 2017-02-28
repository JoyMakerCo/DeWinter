using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButtonController : MonoBehaviour {

    public LevelManager levelManager; //Filled in by the PopUpManager
    public EventInventory eventInventory; //Filled in by the PopUpManager
    public OutfitInventory outfitInventory; //Filled in by the PopUpManager
    public AccessoryInventory accessoryInventory; //Filled in by the PopUpManager
    public DismissPopUp dismiss;
	
	// This performs all the actions necessary for starting the game;
	public void StartGame() {
        eventInventory.StockFullInventory();
        outfitInventory.StockInventory();
        accessoryInventory.StockInventory();
        dismiss.Dismiss();
        levelManager.LoadLevel("Game_Estate");
	}
}
