using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButtonController : MonoBehaviour {

    LevelManager levelManager;
    EventInventory eventInventory;
    OutfitInventory outfitInventory;
    AccessoryInventory accessoryInventory;
    public DismissPopUp dismiss;

	// Use this for initialization
	void Start () {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        eventInventory = GameObject.Find("EventInventory").GetComponent<EventInventory>();
        outfitInventory = GameObject.Find("OutfitInventory").GetComponent<OutfitInventory>();
        accessoryInventory = GameObject.Find("AccessoryInventory").GetComponent<AccessoryInventory>();
    }
	
	// This performs all the actions necessary for starting the game;
	public void StartGame() {
        eventInventory.StockFullInventory();
        outfitInventory.StockInventory();
        accessoryInventory.StockInventory();
        dismiss.Dismiss();
        levelManager.LoadLevel("Game_Estate");
	}
}
