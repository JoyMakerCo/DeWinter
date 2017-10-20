using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

namespace Ambition
{
	[Serializable]
	public struct SpriteMap
	{
		public string ID;
		public Sprite Sprite;
	}

	public class SpriteConfig : ScriptableObject
	{
		public SpriteMap[] Sprites;

		public Sprite GetSprite(string ID)
		{
			SpriteMap map = Array.Find(Sprites, s => s.ID == ID);
			return default(SpriteMap).Equals(map) ? null : map.Sprite;
		}

		public string GetID(Sprite sprite)
		{
			SpriteMap map = Array.Find(Sprites, s => s.Sprite == sprite);
			return default(SpriteMap).Equals(map) ? null : map.ID;
		}

		[MenuItem("Assets/Create/Create Sprite Config")]
		public static void CreatePrefabConfig()
		{
			ScriptableObjectUtil.CreateScriptableObject<SpriteConfig>();
		}
	}
}
