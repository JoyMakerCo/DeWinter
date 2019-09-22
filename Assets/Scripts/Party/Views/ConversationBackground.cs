using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class ConversationBackground : MonoBehaviour
	{
		public SpriteConfig BackgroundCollection;
		private Image _background;
		void Awake ()
		{
			_background = GetComponent<Image>();
			AmbitionApp.Subscribe<RoomVO>(HandleRoom);
			//HandleRoom(AmbitionApp.GetModel<MapModel>().Room);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
		}

		private void HandleRoom(RoomVO room)
		{
			if (room != null)
			{
				if (room.Background == null)
				{
					Sprite[] sprites = BackgroundCollection.GetSpritesByTag("party");
                    room.Background = Util.RNG.TakeRandom(sprites);
				}
				_background.sprite = room.Background;
			}
		}
	}
}
