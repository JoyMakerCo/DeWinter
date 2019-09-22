using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class SandboxInit : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            Core.App.Register<Core.MessageSvc>();
        }
    }
}
