using UnityEngine;
using UnityEngine.UI;
using Ambition;

public class OutfitButtonMediator : MonoBehaviour
{
    public Image ChoicePip; // This is the icon that appears next the item to make them all easier to read. Necessary for the event triggers
    public Sprite ChoicePipUp;
    public Sprite ChoicePipHover;
    public Sprite ChoicePipSelected;

    public Text ItemName;
    public GameObject GiftIndicator;
    public enum InventoryType { Player, Shop, Loadout };
    public InventoryType InventoryOwner; //Necessary, because selecting things in the party loadout screen also equips them, not just displays them. 
    //In theory we could just 'equip' the items in every screen, but I feel like that's just ASKING for a nasty bug, further down the road

    private ItemVO _item;
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void MouseOver()
    {
        ChoicePip.sprite = ChoicePipHover;
    }

    public void MouseLeaves()
    {
        ChoicePip.sprite = ChoicePipUp;
    }

    public void MouseClick()
    {
        ChoicePip.sprite = ChoicePipSelected;
        if (_item != null)
        {
            if (InventoryOwner == InventoryType.Loadout)
            {
                AmbitionApp.SendMessage(InventoryMessages.EQUIP, _item);
            }
            AmbitionApp.SendMessage(InventoryMessages.DISPLAY_ITEM, _item);
        }
    }

    public void SetItem(ItemVO item)
    {
        _item = item;
        ItemName.text = item.Name;
        GiftIndicator.SetActive(false); //Gifts aren't a thing yet
    }

    public void SetInventoryOwner(string owner)
    {
        switch (owner)
        {
            case "Player":
                InventoryOwner = InventoryType.Player;
                break;
            case "Shop":
                InventoryOwner = InventoryType.Shop;
                break;
            case "Loadout":
                InventoryOwner = InventoryType.Loadout;
                break;
        }
    }
}
