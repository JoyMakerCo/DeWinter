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
        private List<ItemVO> _items;

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
            if (_inventory.Inventory.TryGetValue(ItemType.Outfit, out _items))
                Sort(ItemConsts.NOVELTY, false);
            else Array.ForEach(DrawerButtons, b => b.SetItem(null));
        }

		private void Sort(string SortOn, bool Ascending)
		{
			int count = _items.Count;
            if (SortOn == ItemConsts.STYLE)
                _items.Sort((a,b)=>StrCmp(a,b,SortOn,Ascending));
            else _items.Sort((a, b) => IntCmp(a, b, SortOn, Ascending));
            for (int i=DrawerButtons.Length-1; i>=0; i--)
			{
				DrawerButtons[i].SetItem(i < count ? _items[i] : null);
			}
		}

        private int IntCmp(ItemVO a, ItemVO b, string stat, bool ascending)
        {
            string str = null;
            int ia = (a.State?.TryGetValue(stat, out str) ?? false) ? int.Parse(str) : 0;
            int ib = (b.State?.TryGetValue(stat, out str) ?? false) ? int.Parse(str) : 0;
            return ia == ib ? 0 : (ascending && ia > ib) ? 1 : -1;
        }

        private int StrCmp(ItemVO a, ItemVO b, string stat, bool ascending)
        {
            string sa = null, sb = null;
            if (!a.State?.TryGetValue(stat, out sa) ?? false) sa = "";
            if (!b.State?.TryGetValue(stat, out sb) ?? false) sb = "";
            return (ascending?sa:sb).CompareTo(ascending?sb:sa);
        }
    }
}
