using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

#if (UNITY_EDITOR)
using UnityEditor;
#endif

namespace Ambition
{
	[Serializable]
	public struct SpriteMap
	{
		public string ID;
		public Sprite Sprite;
		public string[] Tags;
	}

	public class SpriteConfig : ScriptableObject
	{
		public SpriteMap[] Sprites;

		public Sprite GetSprite(string ID)
		{
            int index = Array.FindIndex(Sprites, s => s.ID == ID);
            return index < 0 ? null : Sprites[index].Sprite;
		}

		public string GetID(Sprite sprite)
		{
			SpriteMap map = Array.Find(Sprites, s => s.Sprite == sprite);
			return default(SpriteMap).Equals(map) ? null : map.ID;
		}

		public Sprite[] GetSpritesByTag(params string[] tags) //Candidates must match all criteria
		{
            List<Sprite> spirtes = new List<Sprite>();
            foreach(SpriteMap s in Sprites)
            {
                if (Array.TrueForAll(tags, t=>Array.IndexOf(s.Tags, t) >= 0))
                {
                    spirtes.Add(s.Sprite);
                }
            }
            return spirtes.ToArray();
		}

#if (UNITY_EDITOR)
		[MenuItem("Assets/Create/Create Sprite Config")]
		public static void CreatePrefabConfig()
		{
			ScriptableObjectUtil.CreateScriptableObject<SpriteConfig>();
		}
#endif
	}
}
