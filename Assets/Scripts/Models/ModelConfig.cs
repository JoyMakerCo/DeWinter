using System;
using UnityEngine;
using Core;

namespace Ambition
{
    public class ModelConfig : MonoBehaviour
    {
        public ScriptableObject[] Models;

        private void Awake()
        {
            ModelSvc svc = App.Register<ModelSvc>();
            Array.ForEach(Models, m => (m as IModelConfig)?.Register(svc));
        }
    }
}
