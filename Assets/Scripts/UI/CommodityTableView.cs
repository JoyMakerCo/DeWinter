using System;
using UnityEngine;
using System.Collections;

namespace Ambition
{
    public class CommodityTableView : MonoBehaviour
    {
        private const float ROW_HEIGHT = 28f;

        public SpriteConfig CommodityConfig;
        public GameObject RowPrefab;

        void Start()
        {
            Clear();
        }

        public void SetCommodities(CommodityVO[] commodities)
        {
            Clear();
            Array.ForEach(commodities, AddCommidity);
        }

        public void AddCommidity(CommodityVO commodity)
        {
            GameObject row = Instantiate(RowPrefab, new Vector3(0f, transform.childCount * ROW_HEIGHT, 0f), Quaternion.identity, this.transform);
            CommodityRowView view = row.GetComponent<CommodityRowView>();
            view.SetCommodity(commodity, CommodityConfig.GetSprite(commodity.ID));
        }

        public void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
