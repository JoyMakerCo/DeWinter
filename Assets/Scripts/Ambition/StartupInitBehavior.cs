using UnityEngine;
using System.Collections;
using Core;

namespace Ambition
{
    public class StartupInitBehavior : MonoBehaviour
    {
        public InputBlocker Blocker;
        public HeaderMediator Header;

        void Awake()
        {
            CommandSvc cmd = App.Register<CommandSvc>();
            App.Register<LocalizationSvc>();
            App.Register<MessageSvc>();
            Blocker.enabled = true;
            Header.enabled = true;
            Destroy(this);
        }
    }
}
