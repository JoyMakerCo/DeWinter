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

        Core.LocalizationModel localizationModel = AmbitionApp.GetModel<Core.LocalizationModel>();

        public void SetCommodity(CommodityVO commodity, Sprite icon)
        {
            Icon.sprite = icon;
            CommodityTF.text = _commodityName(commodity);
            ValueTF.text = "+" + commodity.Value.ToString();
        }

        //To Do: Clean this up when CommodityVO uses RewardVO instead
        private string _commodityName(CommodityVO commodity)
        {
            string name;
            if(commodity.Type.ToString() == "Reputation") //Reputation is an edge case in terms of naming conventions
            {
                if (commodity.ID == "null")
                {
                    name = localizationModel.GetString("commodity." + commodity.Type.ToString().ToLower() + ".name");
                } else
                {
                    name = localizationModel.GetString("commodity.reputation" + commodity.ID.ToLower() + ".name");
                }
            } else if (commodity.Type.ToString() == "Gossip") //The commodity VO doesn't let us identify the tier and the faction of the Gossip until its already been issued
            {
                name = localizationModel.GetString("commodity." + commodity.Type.ToString().ToLower() + ".name");
            } else
            {
                name = localizationModel.GetString("commodity." + commodity.ID + ".name");
            }
            return name;
        }
    }
}
