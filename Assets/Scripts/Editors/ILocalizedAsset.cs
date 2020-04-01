using System;
using System.Collections.Generic;
namespace AmbitionEditor
{
    public interface ILocalizedAsset
    {
#if UNITY_EDITOR
        Dictionary<string, string> Localize();
#endif
        string LocalizationKey { get; set; }
    }
}
