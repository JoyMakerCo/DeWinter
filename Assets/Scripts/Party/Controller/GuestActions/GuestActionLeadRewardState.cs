﻿using System;
using System.Collections.Generic;
using UFlow;
using UnityEngine;

namespace Ambition
{
    public class GuestActionLeadRewardState : UState
    {
        public override void OnEnterState()
        {
/*            UController controller = _Machine._UFlow.GetController(_Machine);
            if (controller !=  null)
            {                
                MapModel map = AmbitionApp.GetModel<MapModel>();
                GuestVO guest = map.Room.Guests[controller.transform.GetSiblingIndex()];
                if (guest.Action != null && guest.Action.Tags != null && guest.Action.Tags.Length > 0)
                {
                    switch(guest.Action.Tags[0])
                    {
                        case "Map":
                            RoomVO[] rooms = Array.FindAll(map.Map.Rooms, r => !r.Revealed);
                            RoomVO room = rooms[ Util.RNG.Generate(rooms.Length)];
                            room.Revealed = true;
                            AmbitionApp.SendMessage(room);
                            break;
                        case "Wine":
                            AmbitionApp.SendMessage(PartyMessages.REFILL_DRINK);
                            break;
                        case "Remarks":
                            AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                            AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                            AmbitionApp.SendMessage(PartyMessages.FREE_REMARK);
                            break;
                        case "Accolade":
                            foreach (GuestVO g in map.Room.Guests)
                            {
                                g.Opinion += (g == guest ? 30 : 10);
                                if (g.Opinion >= 100)
                                {
                                    g.Opinion = 100;
                                    guest.State = GuestState.Charmed;
                                }
                            }
                            break;
                    }
                }
            }
*/        }
    }
}
