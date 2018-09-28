using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ambition
{
    public class CommodityRowView : MonoBehaviour
    {
        public Image Icon;
        public Text CommodityTF;
        public Text ValueTF;

        public void SetCommodity(CommodityVO commodity, Sprite icon)
        {
            Icon.sprite = icon;
            CommodityTF.text = commodity.ID ?? commodity.Type.ToString();
            ValueTF.text = commodity.Value.ToString();
        }
    }
}
