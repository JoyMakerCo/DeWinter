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
			HandleRoom(AmbitionApp.GetModel<MapModel>().Room);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<RoomVO>(HandleRoom);
		}

		private void HandleRoom(RoomVO room)
		{
			if (room != null)
			{
				Sprite spt=null;
				if (room.Background != null) spt = BackgroundCollection.GetSprite(room.Background);
				if (spt == null)
				{
					Sprite[] sprites = BackgroundCollection.GetSpritesByTag("party");
					spt = sprites[UnityEngine.Random.Range(0,sprites.Length)];
					room.Background = BackgroundCollection.GetID(spt);
				}
				_background.sprite = spt;
			}
		}
	}
}
