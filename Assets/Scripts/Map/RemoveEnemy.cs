using System;
using Core;

namespace Ambition
{
	public class RemoveEnemy : ICommand<EnemyVO>
	{
		private bool terminate=false;

		public void Execute (EnemyVO enemy)
		{
			RemoveEnemyFromRoom(AmbitionApp.GetModel<MapModel>().Room, enemy);
        }

        private void RemoveEnemyFromRoom(RoomVO room, EnemyVO enemy)
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