#if UNITY_EDITOR
using System;
using UnityEditor;
namespace Ambition
{
    public class AmbitionSaveBehavior : AssetModificationProcessor
    {
        static string[] OnWillSaveAssets(string [] assets)
        {
            bool updateLocalizations = false;
            foreach (string asset in assets)
            {
                IncidentConfig config = AssetDatabase.LoadAssetAtPath<IncidentConfig>(asset);
                if (config != null)
                {
                    updateLocalizations = true;
                    config.PublishLocalizations();
                }
            }
            if (updateLocalizations)
            {
                LocalizationConfig.UpdateLocalizationFile();
            }
            return assets;
        }
    }
}
#endif
