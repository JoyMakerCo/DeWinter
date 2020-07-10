using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class StartSceneView : MonoBehaviour
    {
        public Dropdown LocSelector;

        private TextAsset[] _locs;
        private LocalizationSvc _svc;

        void Start()
        {
            _locs = Resources.LoadAll<TextAsset>(Filepath.LOCALIZATIONS);
            List<string> languages = new List<string>();
            LocSelector.ClearOptions();
            foreach (TextAsset asset in _locs) languages.Add(asset.name);
            LocSelector.AddOptions(languages);
            _svc = App.Register<LocalizationSvc>();
            int index = languages.IndexOf(_svc.LanguageCode);
            LocSelector.value = (index >= 0) ? index : 0;
            HandleLoc(); // TODO: Save loc selection
        }

        public void HandleLoc()
        {
            TextAsset loc = _locs[LocSelector.value];
            if (loc != null) _svc.LoadLocFile(loc.name, loc.text);
        }
    }
}
