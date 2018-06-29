using System;
using System.Collections.Generic;
using UFlow;
using Core;

namespace Ambition
{
	public class ResetGuestActionState : UState
    {
        override public void OnEnterState()
        {
            UController controller = _machine._uflow.GetController(_machine);
            MapModel map = AmbitionApp.GetModel<MapModel>();
            map.Room.Guests[controller.transform.GetSiblingIndex()].Action = null;
UnityEngine.Debug.Log("Reset Action " + controller.transform.GetSiblingIndex().ToString());
        }
    }
}
