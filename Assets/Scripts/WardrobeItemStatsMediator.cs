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
        void HandleItemDisplay(ItemVO outfit)
        {
            Debug.Log("WardrobeItemStatsMediator.HandleItemDisplay");
            if (outfit?.Type != ItemType.Outfit) return;

            ItemNameText.text = outfit.Name;
            ItemDescriptionText.text = OutfitWrapperVO.GetDescription(outfit);

            //To Do: Make the modesty and luxury sliders always work in the right direction
            LuxurySlider.value = OutfitWrapperVO.GetLuxury(outfit);
            LuxuryStatText.text = OutfitWrapperVO.GetLuxuryText(outfit);

            ModestySlider.value = OutfitWrapperVO.GetModesty(outfit);
            ModestyStatText.text = OutfitWrapperVO.GetModestyText(outfit);
            //Debug.LogFormat("luxury slider {0} modesty slider {1}", LuxurySlider.value, ModestySlider.value);

            NoveltySlider.value = OutfitWrapperVO.GetNovelty(outfit);
            NoveltyStatText.text = OutfitWrapperVO.GetNoveltyText(outfit);
        }


        void BlankStats()
        {
            ItemNameText.text = "";
            ItemDescriptionText.text = "";
            LuxurySlider.value = 0;
            ModestySlider.value = 0;
            NoveltySlider.value = 0;
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

        private string Map(int stat, string[] phrases) => phrases[(int)(phrases.Length * (.5f + stat * .00499f))]; 
    }
}
