using System;
using Core;

namespace DeWinter
{
	public class RemoveEnemy : ICommand<Enemy>
	{
		private bool terminate=false;

		public void Execute (Enemy enemy)
		{
			RemoveEnemyFromRoom(DeWinterApp.GetModel<MapModel>().Room, enemy);
        }

        private void RemoveEnemyFromRoom(RoomVO room, Enemy enemy)
        {
        	if (!terminate)
        	{
				terminate = (room.Enemies != null && room.Enemies.Remove(enemy));
				if (!terminate && room.Doors != null)
				{
	    			foreach(Door door in room.Doors)
	    			{
	    				RemoveEnemyFromRoom(door.Room, enemy);
	    			}
	    		}
	        }
        }
	}
}