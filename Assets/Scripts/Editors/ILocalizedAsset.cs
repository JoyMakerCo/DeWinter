using System;
using System.Collections.Generic;
namespace AmbitionEditor
{
    public interface ILocalizedAsset
    {
#if UNITY_EDITOR
        Dictionary<string, string> Localize();
        void SetLocalizationKey(string value);
#endif
        string GetLocalizationKey();
    }
}
