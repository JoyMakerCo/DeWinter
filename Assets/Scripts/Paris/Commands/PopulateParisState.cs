using System;
using System.Collections.Generic;
using Util;

namespace Ambition
{
    public class PopulateParisState : UFlow.UState
    {
        public override void OnEnter()
        {
            bool isRendezvous = AmbitionApp.GetModel<CharacterModel>().CreateRendezvousMode;
            List<string> list = isRendezvous
                ? AmbitionApp.Paris.Rendezvous
                : AmbitionApp.Paris.Exploration;
            AmbitionApp.Game.Activity = ActivityType.Paris;
            list.ForEach(l => AmbitionApp.SendMessage(ParisMessages.SHOW_LOCATION, l));
            if (!isRendezvous && AmbitionApp.Paris.Daily != null)
            {
                Array.ForEach(AmbitionApp.Paris.Daily, l => AmbitionApp.SendMessage(ParisMessages.SHOW_LOCATION, l));
            }
        }
    }
}
