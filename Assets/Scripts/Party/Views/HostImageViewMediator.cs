using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class HostImageViewMediator : MonoBehaviour
	{
		public GameObject ArtLibrary;

		// Use this for initialization
		void Start () {
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			NotableVO host = model.Party.host;
			PartyArtLibrary lib = ArtLibrary.GetComponent<PartyArtLibrary>();
			Image image = this.gameObject.GetComponent<Image>();
			Sprite [] sprites = host.IsFemale ? lib.FemaleHostSprites : lib.MaleHostSprites;
			image.sprite = sprites[new System.Random().Next(sprites.Length)];
		}
	}
}
