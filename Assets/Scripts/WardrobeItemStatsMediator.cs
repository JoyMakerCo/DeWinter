using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class WardrobeItemStatsMediator : MonoBehaviour
    {
        public Text ItemNameText;
        public Text ItemDescriptionText;

        public Slider LuxurySlider;
        public Slider ModestySlider;
        public Slider NoveltySlider;

        public Text LuxuryStatText;
        public Text ModestyStatText;
        public Text NoveltyStatText;

        public Image PaperDoll;

        public SpriteConfig DressCollection;

        void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleItemDisplay);
            AmbitionApp.Subscribe(InventoryMessages.BUY_ITEM, BlankStats); //Gotta wipe the slate once the item's been moved
            AmbitionApp.Subscribe(InventoryMessages.SELL_ITEM, BlankStats);
            BlankStats();
        }

        void OnDestroy()
        {
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.DISPLAY_ITEM, HandleItemDisplay);
            AmbitionApp.Unsubscribe(InventoryMessages.BUY_ITEM, BlankStats);
            AmbitionApp.Unsubscribe(InventoryMessages.SELL_ITEM, BlankStats);
        }

        //TO DO: Make this work with Accessories (when they're full implemented)
        void HandleItemDisplay(ItemVO item)
        {
            OutfitVO outfitTest = item as OutfitVO; //This is to test if the item is an Outfit
            if (outfitTest != null)
            {
                OutfitVO outfit = (OutfitVO)item;
                ItemNameText.text = outfit.Name;
                ItemDescriptionText.text = outfit.Description;
                //To Do: Make the modesty and luxury sliders always work in the right direction
                LuxurySlider.value = outfit.Luxury;
                LuxuryStatText.text = outfit.Luxury + " (" + outfit.GenerateLuxuryString() + ")";
                ModestySlider.value = outfit.Modesty;
                ModestyStatText.text = outfit.Modesty + " (" + outfit.GenerateModestyString() + ")";
                NoveltySlider.value = outfit.Novelty;
                NoveltyStatText.text = outfit.Novelty + "/100";
                PaperDoll.sprite = DressCollection.GetSprite(outfit.Style);
            }

        }

        void BlankStats()
        {
            ItemNameText.text = "";
            ItemDescriptionText.text = "";
            LuxurySlider.value = 0;
            ModestySlider.value = 0;
            NoveltySlider.value = 0;
            PaperDoll.sprite = DressCollection.GetSprite("none");
            LuxuryStatText.text = "None";
            ModestyStatText.text = "None";
            NoveltyStatText.text = "None";
            HideTextStats();
        }

        public void ShowLuxuryTextStats()
        {
            LuxuryStatText.gameObject.SetActive(true);
        }

        public void ShowModestyTextStats()
        {
            ModestyStatText.gameObject.SetActive(true);
        }

        public void ShowNoveltyTextStats()
        {
            NoveltyStatText.gameObject.SetActive(true);
        }

        //You only have one mouse, so if your mouse leaves one of the sliders, you can really just hide them all
        public void HideTextStats()
        {
            LuxuryStatText.gameObject.SetActive(false);
            ModestyStatText.gameObject.SetActive(false);
            NoveltyStatText.gameObject.SetActive(false);
        }
    }
}
