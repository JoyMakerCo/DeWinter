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
            CommodityTF.text = _commodityName(commodity);
            ValueTF.text = "+" + commodity.Value.ToString();
        }

        //To Do: Clean this up when CommodityVO uses RewardVO instead
        private string _commodityName(CommodityVO commodity)
        {

            switch (commodity.Type.ToString())
            {
                case "Reputation": //Reputation is an edge case in terms of naming conventions
                    return (string.IsNullOrWhiteSpace(commodity.ID) || commodity.ID == "null")
                        ? AmbitionApp.GetString("commodity." + commodity.Type.ToString().ToLower() + ".name")
                        : AmbitionApp.GetString("commodity.reputation" + commodity.ID.ToLower() + ".name");
                case "Gossip":
                    return AmbitionApp.GetString("commodity." + commodity.Type.ToString().ToLower() + ".name");
            }
            return AmbitionApp.GetString("commodity." + commodity.ID + ".name");
        }
    }
}
