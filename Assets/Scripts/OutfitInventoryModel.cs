using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Core;

// TODO: Feed this to the inventory model
namespace Ambition
{
	public class OutfitInventoryModel : DocumentModel
	{
		private const string DOCUMENT_ID = "OutfitData";

		[JsonProperty("inventory")]
	    public List<Outfit> Inventory;

		public List<Outfit> Merchant = new List<Outfit>();

	    public int Capacity = 5; //The Max Size at Game Start
	    public int MaxCapacity = 9; //The Max Possible Size

	    //Used for seeing if the same Outfit was used twice in a row
		public Outfit LastPartyOutfit;

		public Outfit PartyOutfit;

		public OutfitInventoryModel() : base(DOCUMENT_ID) {}
	}
}