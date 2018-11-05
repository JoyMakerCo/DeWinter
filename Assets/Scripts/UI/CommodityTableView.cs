using System;
using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class CommodityTableView : MonoBehaviour
    {
        public SpriteConfig CommodityConfig;
        public GameObject ContentList;
        public GameObject RowPrefab;

        public void SetCommodities(CommodityVO[] commodities)
        {
            Clear();
            Array.ForEach(commodities, AddCommidity);
        }

        public void AddCommidity(CommodityVO commodity)
        {
            if(commodity.Type != CommodityType.Incident && commodity.Type != CommodityType.Message) //Incidents and Messages don't get listed in the rewards because they aren't supposed to be treated like items
            {
                GameObject row = Instantiate(RowPrefab, ContentList.transform);
                CommodityRowView view = row.GetComponent<CommodityRowView>();
                view.SetCommodity(commodity, CommodityConfig.GetSprite(commodity.Type.ToString()));
            }
        }

        public void Clear()
        {
            foreach (Transform child in ContentList.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
