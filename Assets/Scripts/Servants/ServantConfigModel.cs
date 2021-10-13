using System;
using UnityEngine;
using Core;

namespace Ambition
{
    public class ServantConfigModel : ScriptableObject, IModelConfig
    {
        public ServantConfig[] Servants;

        public void Register(Core.ModelSvc svc)
        {
            ServantModel model = svc.Register<ServantModel>();
            ServantVO servant;
            foreach (ServantConfig config in Servants)
            {
                servant = config.GetServant();
                model.Servants.Add(servant);
            }
        }
#if (UNITY_EDITOR)
        [UnityEditor.MenuItem("Assets/Create/ServantConfigModel")]
        public static void CreateItem()
        {
            string ModelID = "ServantModel";
            Util.ScriptableObjectUtil.GetUniqueInstance<ServantConfigModel>(ModelID);
        }
#endif
    }
}
