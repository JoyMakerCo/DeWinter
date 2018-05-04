using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class WardrobeMediator : MonoBehaviour
	{
		public SortButton[] SortButtons;
		public Button OutfitsButton;
		public Button AccessoriesButton;
		public DrawerButton[] DrawerButtons;

		private InventoryModel _inventory;

		void OnEnable()
		{
			foreach(SortButton b in SortButtons)
			{
				b.onClick.AddListener(delegate{Sort(b.SortOn, b.Ascending);});
			}
//			OutfitsButton.onClick.AddListener(HandleOutfit);
//			AccessoriesButton.onClick.AddListener(HandleAccessory);
		}

		void OnDisable()
		{
			foreach(SortButton b in SortButtons)
			{
				b.onClick.RemoveListener(delegate{Sort(b.SortOn, b.Ascending);});
			}
//			OutfitsButton.onClick.RemoveListener(HandleOutfit);
//			AccessoriesButton.onClick.RemoveListener(HandleAccessory);
		}

		void Start()
		{
			_inventory = AmbitionApp.GetModel<InventoryModel>();
//			Sort(SortButtons[0].SortOn, SortButtons[0].Ascending); 
			for(int i=DrawerButtons.Length-1; i>=0; i--)
			{
				DrawerButtons[i].SetItem(i < _inventory.Inventory.Count ? _inventory.Inventory[i] : null);
			}	
		}

		private void Sort(string SortOn, bool Ascending)
		{
			List<ItemVO> items = _inventory.Inventory.FindAll(i=>i.Type == ItemConsts.OUTFIT);
			int count = items.Count;
			foreach (ItemVO item in items)
				if (!item.State.ContainsKey(SortOn))
					item.State.Add(SortOn, 0);
			if (Ascending)
				items.Sort((a,b)=>(Convert.ToInt32(a.State[SortOn])).CompareTo(Convert.ToInt32(b.State[SortOn])));
			else
				items.Sort((a,b)=>(Convert.ToInt32(b.State[SortOn])).CompareTo(Convert.ToInt32(a.State[SortOn])));

			for(int i=DrawerButtons.Length-1; i>=0; i--)
			{
				DrawerButtons[i].SetItem(i < count ? items[i] : null);
			}
		}
	}
}
