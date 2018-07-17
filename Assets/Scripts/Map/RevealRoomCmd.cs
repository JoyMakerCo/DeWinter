using Core;
using System;

namespace Ambition
{
    public class RevealRoomCmd : ICommand<RoomVO>
    {
        public void Execute (RoomVO room)
        {
            room.Revealed = true;
        }
    }
}
