using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Ambition
{
    public class ModelTrackingSvc : IAppService
    {
        private List<IResettable> _resettable = new List<IResettable>();

        public void Track(IResettable model)
        {
            if (model != null) _resettable.Add(model);
        }

        public void Reset() => _resettable.ForEach(m => m.Reset());
        public void Dispose() => _resettable.Clear();
    }
}
