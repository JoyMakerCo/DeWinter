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
            if (outfit?.Type != ItemType.Outfit) return;

            ItemNameText.text = AmbitionApp.GetString("item." + outfit.Name + ".name");
            ItemDescriptionText.text = AmbitionApp.GetString("item." + outfit.Name + ".description");

            //To Do: Make the modesty and luxury sliders always work in the right direction
            LuxurySlider.value = GetIntStat(outfit, ItemConsts.LUXURY);
            LuxuryStatText.text = Map((int)(LuxurySlider.value), AmbitionApp.GetPhrases("outfit.luxury"));

            ModestySlider.value = GetIntStat(outfit, ItemConsts.MODESTY);
            ModestyStatText.text = Map((int)(ModestySlider.value), AmbitionApp.GetPhrases("outfit.modesty"));

            NoveltySlider.value = GetIntStat(outfit, ItemConsts.NOVELTY);
            string[] phrases = AmbitionApp.GetPhrases("outfit.novelty");
            NoveltyStatText.text = phrases[(int)(phrases.Length * NoveltySlider.value * .01f)];

            string style = null;
        }

        private int GetIntStat(ItemVO outfit, string stat)
        {
            return outfit?.State == null
                ? 0
                : outfit.State.TryGetValue(ItemConsts.LUXURY, out stat)
                ? int.Parse(stat)
                : 0;
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

        private string Map(int stat, string[] phrases) => phrases[(int)(phrases.Length * (.5f + stat * .005f))];
    }
}
