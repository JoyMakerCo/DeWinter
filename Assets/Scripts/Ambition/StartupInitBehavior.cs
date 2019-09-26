using UnityEngine;
using System.Collections;
using Core;

namespace Ambition
{
    public class StartupInitBehavior : MonoBehaviour
    {
        // Use this for initialization
        void Awake()
        {
            App.Register<ModelSvc>();
            App.Register<MessageSvc>();
            App.Register<CommandSvc>();
            App.Register<LocalizationSvc>();
            GetComponent<InputBlocker>().enabled = true;
            Destroy(this);
        }
    }
}
