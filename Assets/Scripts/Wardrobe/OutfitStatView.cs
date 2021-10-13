using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class OutfitStatView : MonoBehaviour
    {
        private const float TIME = .4f;

        public Slider FillBar;
        public Text ValueText;
        public string Stat;

        private void Awake()
        {
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.BROWSE, HandleEquip);
            AmbitionApp.Subscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.BROWSE, HandleEquip);
            AmbitionApp.Unsubscribe<ItemVO>(InventoryMessages.EQUIP, HandleEquip);
        }

        private void HandleEquip(ItemVO item)
        {
            OutfitVO outfit = item as OutfitVO;
            if (outfit != null)
            {
                int value = outfit.GetIntStat(Stat);
                if (Stat != ItemConsts.NOVELTY) value = (int)((100 + value) >> 1);
                StopAllCoroutines();
                ValueText.text = value.ToString();
                StartCoroutine(Fill(value));
            }
        }

        private IEnumerator Fill(int value)
        {
            float k = FillBar.value;
            float s = (float)(value - k) / TIME;
            for (float t=0; t<TIME; t+=Time.deltaTime)
            {
                FillBar.value = t * s + k;
                yield return null;
            }
            FillBar.value = value;
        }
    }
}
