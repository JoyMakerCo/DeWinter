using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Ambition
{
	[Serializable]
	public struct OutfitSprite
	{
		public string style;
		public Sprite sprite;
	}

	public class WardrobeImageController : MonoBehaviour
	{
	    public Image displayImage;
	    
		public OutfitSprite[] OutfitSprites;

	    public string displayID
	    {
	    	set
	    	{
				OutfitSprite spt = Array.Find(OutfitSprites, s => s.style == value);
				if (!OutfitSprite.Equals(spt, default(OutfitSprite)))
				{
					displayImage.sprite = spt.sprite;
				}
				else
				{
					displayImage.sprite = OutfitSprites[0].sprite;
				}
	    	}
	    }
	}
}
