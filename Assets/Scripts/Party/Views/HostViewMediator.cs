using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class HostViewMediator : MonoBehaviour
	{
		public Image HostImage;
		public Text HostText;

		// Use this for initialization
		void Start ()
		{
			PartyArtLibrary ArtLibrary = GetComponentInParent<PartyArtLibrary>();
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			NotableVO host = model.Party.Host;
			PartyArtLibrary lib = ArtLibrary.GetComponent<PartyArtLibrary>();
			Sprite [] sprites = host.IsFemale ? lib.FemaleHostSprites : lib.MaleHostSprites;
			if (host.Variant < 0) host.Variant = new System.Random().Next(sprites.Length);
			HostImage.sprite = sprites[host.Variant];
			HostText.text = host.Name;
		}
	}
}
