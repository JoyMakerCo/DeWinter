using System;
using UnityEngine;
namespace Ambition
{
    public class InventoryConfig : ScriptableObject, IModelConfig
    {
        public float OutOfStyleMultiplier = 0.75f;
        public float SellbackMultiplier = 0.5f;
        public int NumMarketSlots = 5;
        public ItemConfig[] Items;

        [Range(-100, 100)]
        public int RisqueLimit;
        [Range(-100, 100)]
        public int ModestyLimit;
        [Range(-100, 100)]
        public int HumbleLimit;
        [Range(-100, 100)]
        public int LuxuriousLimit;

        public void Register(Core.ModelSvc modelService)
        {
            InventoryModel model = modelService.Register<InventoryModel>();
            model.OutOfStyleMultiplier = this.OutOfStyleMultiplier;
            model.SellbackMultiplier = this.SellbackMultiplier;
            model.NumMarketSlots = this.NumMarketSlots;
            model.LuxuryLimit = LuxuriousLimit;
            model.HumbleLimit = HumbleLimit;
            model.ModestLimit = ModestyLimit;
            model.RisqueLimit = RisqueLimit;
            for (int i = Items.Length - 1; i >= 0; --i)
            {
                model.Import(Items[i]);
            }
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Ambition/Create/InventoryModel")]
        public static void CreateItem() => Util.ScriptableObjectUtil.CreateScriptableObject<InventoryConfig>("Inventory Model");
#endif
    }
}
