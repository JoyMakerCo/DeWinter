using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class SortLabelButtonMediator : MonoBehaviour
    {
        public Text LabelText;
        public Image LabelButton;
        private string _labelString;
        
        public Color LightSortBackgroundColor;
        public Color DarkSortBackgroundColor;
        public Color LightSortTextColor;
        public Color DarkSortTextColor;

        private void Awake()
        {
            MouseLeave();
        }

        public void MouseHover()
        {
            LabelText.color = LightSortTextColor;
            LabelButton.color = DarkSortBackgroundColor;
        }

        public void MouseLeave()
        {
            LabelText.color = DarkSortTextColor;
            LabelButton.color = LightSortBackgroundColor;
        }
                
        public void MouseClick()
        {
            MouseHover();
            AmbitionApp.SendMessage(InventoryMessages.SORT_INVENTORY, _labelString);
        }

        public void SetLabelString(string label)
        {
            _labelString = label;
            LabelText.text = _labelString;
        }
    }
}

