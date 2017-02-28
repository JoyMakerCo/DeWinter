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
				if (!terminate && room.Neighbors != null)
				{
	    			foreach(RoomVO neighbor in room.Neighbors)
	    			{
						RemoveEnemyFromRoom(neighbor, enemy);
	    			}
	    		}
	        }
        }
	}
}