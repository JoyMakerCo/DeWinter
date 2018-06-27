using System;
using UFlow;
using UnityEngine;
using UnityEngine.Events;

namespace UFlow
{
    // TODO: Register arbitrary links with delegates, and nix this class completely
    public class UControllerLink : ULink
    {
        override public void Initialize()
        {
            Invoke();
        }

        protected void Invoke()
        {
            UController controller = _machine._uflow.GetController(_machine);
            // if (controller != null) controller.Invoke(ID);
        }
    }
}
